using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Events.User;
using AttendanceSystem.Domain.Identities;
using AttendanceSystem.Infrastructure.Persistence;
using AttendanceSystem.Infrastructure.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Infrastructure;
public static class DependencyInjection
{
    private const string MIGRATIONS_ASSEMBLY = "AttendanceSystem.Migrator";
    private const string INFRASTRUCTURE_ASSEMBLY = "AttendanceSystem.Infrastructure";
    private const string APPLICATION_ASSEMBLY = "AttendanceSystem.Application";
    private const string DOMAIN_ASSEMBLY = "AttendanceSystem.Domain";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddIdentityServices();
        services.AddRepositories();
        services.AddServices();
        services.AddHttpServices();
        services.AddLoggerConfiguration(configuration);
        services.AddMasstransitServices(configuration);
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        return services;
    }

    private static IServiceCollection AddLoggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var seqServerUrl = configuration.GetValue<string>("Seq:ServerUrl") ?? throw new InvalidOperationException("Seq Server URL not configured");
        var seqApiKey = configuration.GetValue<string>("Seq:ApiKey") ?? throw new InvalidOperationException("Seq API Key not configured");
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithThreadId()
            .Enrich.WithEnvironmentName()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq(serverUrl: seqServerUrl, apiKey: seqApiKey)
            .CreateLogger();

        services.AddSerilog();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var domainAssembly = Assembly.Load(DOMAIN_ASSEMBLY);
        var assembly = Assembly.Load(INFRASTRUCTURE_ASSEMBLY);

        var repositoryInterfaces = domainAssembly.GetTypes()
            .Where(t => t.Name.EndsWith("Repository") && t.IsInterface)
            .ToList();
        foreach (var repositoryInterface in repositoryInterfaces)
        {
            var implementation = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == repositoryInterface.Name.Substring(1) && repositoryInterface.IsAssignableFrom(t));
            if (implementation != null)
            {
                services.AddScoped(repositoryInterface, implementation);
            }
        }

        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;            
            options.SignIn.RequireConfirmedEmail = false;
        })
        .AddRoles<AppRole>()
        .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.Load(APPLICATION_ASSEMBLY);
        var assembly = Assembly.GetExecutingAssembly();        
        
        var serviceInterfaces = applicationAssembly.GetTypes()
            .Where(t => t.Name.EndsWith("Service") && t.IsInterface)
            .ToList();
        foreach (var serviceInterface in serviceInterfaces)
        {
            var implementation = assembly.GetTypes()
                .FirstOrDefault(t => t.Name == serviceInterface.Name.Substring(1) && serviceInterface.IsAssignableFrom(t));
            if (implementation != null)
            {
                services.AddScoped(serviceInterface, implementation);
            }
        }
        return services;
    }

    private static IServiceCollection AddHttpServices(this IServiceCollection services)
    {        
        services.AddScoped<IUserContextProvider, HttpUserContextProvider>();
        return services;
    }

    private static IServiceCollection AddMasstransitServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var host = configuration.GetValue<string>("RabbitMQ:Host") ?? throw new InvalidOperationException("RabbitMQ Host is not configured");
                var username = configuration.GetValue<string>("RabbitMQ:Username") ?? throw new InvalidOperationException("RabbitMQ Username is not configured");
                var password = configuration.GetValue<string>("RabbitMQ:Password") ?? throw new InvalidOperationException("RabbitMQ Password is not configured");
                cfg.Host(host, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.ConfigureEndpoints(context);
                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                cfg.UseCircuitBreaker(r =>
                {
                    r.TrackingPeriod = TimeSpan.FromMinutes(1);
                    r.TripThreshold = 15;
                    r.ActiveThreshold = 10;
                    r.ResetInterval = TimeSpan.FromMinutes(5);
                });
            });
            x.SetKebabCaseEndpointNameFormatter();
        });

        return services;
    }
}
