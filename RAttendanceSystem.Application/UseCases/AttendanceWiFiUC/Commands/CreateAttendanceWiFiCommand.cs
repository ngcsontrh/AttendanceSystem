using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Commands
{
    public record CreateAttendanceWiFiCommand(
        string Location,
        string SSID,
        string BSSID,
        TimeOnly ValidCheckInTime,
        TimeOnly ValidCheckOutTime,
        string? Description = null
    );

    public record CreateAttendanceWiFiResponse(Guid Id);
}
