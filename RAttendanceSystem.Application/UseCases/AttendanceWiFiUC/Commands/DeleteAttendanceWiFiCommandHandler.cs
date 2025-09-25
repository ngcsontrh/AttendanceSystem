using System;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Commands
{
    public class DeleteAttendanceWiFiCommandHandler
    {
        private readonly IAttendanceWiFiRepository _attendanceWiFiRepository;
        private readonly ILogger<DeleteAttendanceWiFiCommandHandler> _logger;

        public DeleteAttendanceWiFiCommandHandler(IAttendanceWiFiRepository attendanceWiFiRepository, ILogger<DeleteAttendanceWiFiCommandHandler> logger)
        {
            _attendanceWiFiRepository = attendanceWiFiRepository;
            _logger = logger;
        }

        public async Task HandleAsync(DeleteAttendanceWiFiCommand command)
        {
            try
            {
                var attendanceWiFi = await _attendanceWiFiRepository.GetRecordAsync(command.Id);
                if (attendanceWiFi == null)
                {
                    _logger.LogWarning("AttendanceWiFi with ID {AttendanceWiFiId} not found.", command.Id);
                    throw new RecordNotFoundException($"AttendanceWiFi with ID {command.Id} not found.");
                }
                _attendanceWiFiRepository.Delete(attendanceWiFi);
                await _attendanceWiFiRepository.SaveChangesAsync();
                _logger.LogInformation("AttendanceWiFi with ID {AttendanceWiFiId} deleted successfully.", command.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting AttendanceWiFi with ID {AttendanceWiFiId}.", command.Id);
                throw;
            }
        }
    }
}
