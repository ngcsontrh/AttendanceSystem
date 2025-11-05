using AttendanceSystem.Application.Commons.Services;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Infrastructure.Services
{
    public class MessagingService : IMessagingService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessagingService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            await _publishEndpoint.Publish(message);
        }
    }
}
