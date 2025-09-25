using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Queries
{
    public class GetListAttendanceWiFiQueryHandler
    {
        private readonly IAttendanceWiFiRepository _attendanceWiFiRepository;
        private readonly ILogger<GetListAttendanceWiFiQueryHandler> _logger;

        public GetListAttendanceWiFiQueryHandler(IAttendanceWiFiRepository attendanceWiFiRepository, ILogger<GetListAttendanceWiFiQueryHandler> logger)
        {
            _attendanceWiFiRepository = attendanceWiFiRepository;
            _logger = logger;
        }

        public async Task<GetListAttendanceWiFiResponse> Handle(GetListAttendanceWiFiQuery model)
        {
            var attendanceWiFis = await _attendanceWiFiRepository.GetListAsync(x => true);
            var response = MapToResponse(attendanceWiFis);
            _logger.LogInformation("Retrieved {Count} AttendanceWiFi records", attendanceWiFis.Count);
            return response;
        }

        private GetListAttendanceWiFiResponse MapToResponse(IReadOnlyList<AttendanceWiFi> attendanceWiFis)
        {
            var data =  attendanceWiFis.Select(x => new GetListAttendanceWiFiResponseData(
                x.Id,
                x.Location,
                x.SSID,
                x.BSSID,
                x.Description)).ToList();
            return new GetListAttendanceWiFiResponse(data);
        }
    }
}
