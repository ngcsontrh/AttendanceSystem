using AttendanceSystem.Domain.Events.User;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Worker.Mail.Consumers
{
    public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly ILogger<UserCreatedEventConsumer> _logger;

        public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            _logger.LogInformation("UserId: {id}", context.Message.UserId);
            _logger.LogInformation("UserName: {name}", context.Message.UserName);
            _logger.LogInformation("UserEmail: {email}", context.Message.UserEmail);
            _logger.LogInformation("UserCreatedEvent consumed: {Message}", context.Message);
            await Task.CompletedTask;
        }
    }
}
