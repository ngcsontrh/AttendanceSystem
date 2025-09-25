using RAttendanceSystem.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Services
{
    internal class MailService : IMailService
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            return Task.CompletedTask;
        }
    }
}
