using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Queries
{
    public record GetAttendanceWiFiDetailQuery(Guid Id);

    public record GetAttendanceWiFiDetailResponse(
        Guid Id,
        string Location,
        string SSID,
        string BSSID,
        TimeOnly ValidCheckInTime,
        TimeOnly ValidCheckOutTime,
        string? Description,
        DateTime CreatedAt
    );
}
