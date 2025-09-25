using System;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Commands
{
    public class CreateAttendanceWiFiCommandHandler
    {
        private readonly IAttendanceWiFiRepository _attendanceWiFiRepository;
        private readonly ILogger<CreateAttendanceWiFiCommandHandler> _logger;

        public CreateAttendanceWiFiCommandHandler(IAttendanceWiFiRepository attendanceWiFiRepository, ILogger<CreateAttendanceWiFiCommandHandler> logger)
        {
            _attendanceWiFiRepository = attendanceWiFiRepository;
            _logger = logger;
        }

        public async Task<CreateAttendanceWiFiResponse> HandleAsync(CreateAttendanceWiFiCommand command)
        {
            try
            {
                var newAttendanceWiFi = new AttendanceWiFi
                {
                    Id = Guid.CreateVersion7(),
                    Location = command.Location,
                    SSID = command.SSID,
                    BSSID = command.BSSID,
                    Description = command.Description,
                    CreatedAt = DateTime.UtcNow
                };
                _attendanceWiFiRepository.Add(newAttendanceWiFi);
                await _attendanceWiFiRepository.SaveChangesAsync();
                _logger.LogInformation("Created new AttendanceWiFi with ID: {AttendanceWiFiId}", newAttendanceWiFi.Id);
                return new CreateAttendanceWiFiResponse(newAttendanceWiFi.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new AttendanceWiFi.");
                throw;
            }
        }
    }
}
