using AttendanceSystem.Api.Commons;
using AttendanceSystem.Application.Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AttendanceSystem.Api;

public static class DependencyInjection
{
    public static IEndpointRouteBuilder MapEndpointDefinitions(this IEndpointRouteBuilder routes)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        var endpointRegisters = assembly.GetTypes()
            .Where(t => typeof(IEndpointRegister).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => (IEndpointRegister)Activator.CreateInstance(t)!)
            .ToList();
        foreach (var register in endpointRegisters)
        {
            register.MapRoutes(routes);
        }
        return routes;
    }

    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings") ?? throw new InvalidOperationException("JWT Settings not configured");
        var secretKey = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret key not configured");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }

    public static IServiceCollection AddCorsPolicies(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(AppConstraint.StaffPolicy, policy =>
            {
                policy.RequireRole(
                    AppConstraint.StaffRole,
                    AppConstraint.ManagerRole,
                    AppConstraint.AdminRole
                    );
                })
            .AddPolicy(AppConstraint.ManagerPolicy, policy =>
            {
                policy.RequireRole(
                    AppConstraint.ManagerRole,
                    AppConstraint.AdminRole
                );
            })
            .AddPolicy(AppConstraint.AdminPolicy, policy =>
            {
                policy.RequireRole(AppConstraint.AdminRole);
            });
        return services;
    }

    public static WebApplication UseCorsPolicies(this WebApplication app)
    {        
        app.UseCors("AllowAll");
        return app;
    }
}
