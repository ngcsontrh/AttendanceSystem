using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Services
{
    public interface IMessagingService
    {
        Task PublishAsync<T>(T message) where T : class;
    }
}
