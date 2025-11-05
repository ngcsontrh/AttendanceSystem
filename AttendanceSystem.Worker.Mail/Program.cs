using AttendanceSystem.ServiceDefaults;
using AttendanceSystem.Worker.Mail;
using AttendanceSystem.Worker.Mail.Configs;
using AttendanceSystem.Worker.Mail.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

builder.AddServiceDefaults();

builder.Services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<EmailSettings>>().Value
);

builder.Services.AddMassTransit(x =>
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
    });
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<UserCreatedEventConsumer>();
});

var host = builder.Build();
host.Run();
