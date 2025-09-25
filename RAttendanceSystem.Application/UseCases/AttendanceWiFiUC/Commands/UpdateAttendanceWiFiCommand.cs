using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceWiFiUC.Commands
{
    public record UpdateAttendanceWiFiCommand(
        Guid Id,
        string Location,
        string SSID,
        string BSSID,
        string? Description = null
    );
}
