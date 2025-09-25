using RAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceUC.Queries
{
    public record GetAttendanceHistoryQuery(
        Guid? EmployeeId = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        DateTime? Day = null,
        int? Year = null,
        int? Month = null,
        string? Location = null
    );

    public record GetAttendanceHistoryResponse(
        List<AttendanceHistoryResponseData> Data,
        int TotalRecords
    );

    public record AttendanceHistoryResponseData(
        Guid Id,
        Guid EmployeeId,
        DateTime CreatedAt,
        DateTime? CheckInTime = null,
        DateTime? CheckOutTime = null,
        AttendanceStatus? CheckInStatus = null,
        AttendanceStatus? CheckOutStatus = null
    );
}