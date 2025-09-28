using System;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Commands
{
    public class UpdateAttendanceWiFiCommandHandler
    {
        private readonly IAttendanceWiFiRepository _attendanceWiFiRepository;
        private readonly ILogger<UpdateAttendanceWiFiCommandHandler> _logger;

        public UpdateAttendanceWiFiCommandHandler(IAttendanceWiFiRepository attendanceWiFiRepository, ILogger<UpdateAttendanceWiFiCommandHandler> logger)
        {
            _attendanceWiFiRepository = attendanceWiFiRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateAttendanceWiFiCommand command)
        {
            try
            {
                var attendanceWiFi = await _attendanceWiFiRepository.GetRecordAsync(command.Id);
                if (attendanceWiFi == null)
                {
                    _logger.LogWarning("AttendanceWiFi with ID {AttendanceWiFiId} not found.", command.Id);
                    throw new RecordNotFoundException($"AttendanceWiFi with ID {command.Id} not found.");
                }
                MapToEntity(attendanceWiFi, command);
                await _attendanceWiFiRepository.SaveChangesAsync();
                _logger.LogInformation("AttendanceWiFi with ID {AttendanceWiFiId} updated successfully.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating AttendanceWiFi with ID {AttendanceWiFiId}.", command.Id);
                throw;
            }
        }

        void MapToEntity(AttendanceWiFi entity, UpdateAttendanceWiFiCommand command)
        {
            entity.Location = command.Location;
            entity.SSID = command.SSID;
            entity.BSSID = command.BSSID;
            entity.Description = command.Description;
            entity.ValidCheckInTime = command.ValidCheckInTime;
            entity.ValidCheckOutTime = command.ValidCheckOutTime;
        }
    }
}
