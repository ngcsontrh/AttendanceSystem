using AttendanceSystem.Domain.Events.User;
using AttendanceSystem.Worker.Mail.Configs;
using AttendanceSystem.Worker.Mail.Utils;
using MailKit.Net.Smtp;
using MassTransit;
using MimeKit;
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
        private readonly EmailSettings _emailSettings;

        public UserCreatedEventConsumer(ILogger<UserCreatedEventConsumer> logger, EmailSettings emailSettings)
        {
            _logger = logger;
            _emailSettings = emailSettings;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            try
            {
                _logger.LogInformation("Đang xử lý sự kiện UserCreatedEvent cho người dùng {UserId} - {UserEmail}", context.Message.UserId, context.Message.UserEmail);
                StringBuilder mailContent = MailHelper.LoadWelcomeMailTemplate();
                mailContent.Replace("{{UserName}}", context.Message.UserName);
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Hệ thống chấm công", _emailSettings.Username));
                message.To.Add(new MailboxAddress(context.Message.UserName, context.Message.UserEmail));
                message.Subject = "Chào mừng bạn đến với hệ thống chấm công";
                message.Body = new TextPart("html")
                {
                    Text = mailContent.ToString()
                };
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                _logger.LogInformation("Gửi email chào mừng đến người dùng {UserId} - {UserEmail} thành công", context.Message.UserId, context.Message.UserEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Có lỗi xảy ra khi gửi email chào mừng đến người dùng {UserId} - {UserEmail}", context.Message.UserId, context.Message.UserEmail);
                throw;
            }            
        }
    }
}
