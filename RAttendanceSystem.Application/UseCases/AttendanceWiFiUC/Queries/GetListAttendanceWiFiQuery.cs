using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Queries
{
    public record GetListAttendanceWiFiQuery();

    public record GetListAttendanceWiFiResponse(
        List<GetListAttendanceWiFiResponseData> Data
    );

    public record GetListAttendanceWiFiResponseData(
        Guid Id,
        string Location,
        string SSID,
        string BSSID,
        TimeOnly ValidCheckInTime,
        TimeOnly ValidCheckOutTime,
        string? Description = null
    );
}
