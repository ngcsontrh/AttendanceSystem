using RAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceUC.Commands
{
    public record CheckInCommand(
        Guid EmployeeId,
        string SSID,
        string BSSID
    );

    public record CheckOutCommand(
        Guid EmployeeId,
        string SSID,
        string BSSID
    );

    public record AttendanceResponse(
        Guid Id,
        AttendanceStatus Status,
        DateTime Timestamp
    );
}