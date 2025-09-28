using System;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Queries
{
    public class GetAttendanceWiFiDetailQueryHandler
    {
        private readonly IAttendanceWiFiRepository _attendanceWiFiRepository;
        private readonly ILogger<GetAttendanceWiFiDetailQueryHandler> _logger;

        public GetAttendanceWiFiDetailQueryHandler(IAttendanceWiFiRepository attendanceWiFiRepository, ILogger<GetAttendanceWiFiDetailQueryHandler> logger)
        {
            _attendanceWiFiRepository = attendanceWiFiRepository;
            _logger = logger;
        }

        public async Task<GetAttendanceWiFiDetailResponse?> HandleAsync(GetAttendanceWiFiDetailQuery model)
        {
            try
            {
                var attendanceWiFi = await _attendanceWiFiRepository.GetRecordAsync(model.Id);
                if (attendanceWiFi == null)
                {
                    throw new RecordNotFoundException($"AttendanceWiFi with ID {model.Id} not found.");
                }
                return MapToResponse(attendanceWiFi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting AttendanceWiFi detail for ID: {AttendanceWiFiId}", model.Id);
                throw;
            }
        }

        private GetAttendanceWiFiDetailResponse MapToResponse(AttendanceWiFi attendanceWiFi)
        {
            return new GetAttendanceWiFiDetailResponse(
                attendanceWiFi.Id,
                attendanceWiFi.Location,
                attendanceWiFi.SSID,
                attendanceWiFi.BSSID,
                attendanceWiFi.ValidCheckInTime,
                attendanceWiFi.ValidCheckOutTime,
                attendanceWiFi.Description,
                attendanceWiFi.CreatedAt
            );
        }
    }
}
