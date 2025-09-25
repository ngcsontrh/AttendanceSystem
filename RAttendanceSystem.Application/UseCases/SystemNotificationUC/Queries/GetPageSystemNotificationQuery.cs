using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.SystemNotificationUC.Queries
{
    public record GetPageSystemNotificationQuery(
        Guid ReceiverId,
        int PageNumber,
        int PageSize
    );

    public record GetPageSystemNotificationResponse(
        List<GetPageSystemNotificationResponseData> Data,
        int TotalRecords
    );

    public record GetPageSystemNotificationResponseData(
        Guid Id,
        string Title,
        string? Content,
        DateTime CreatedAt
    );
}
