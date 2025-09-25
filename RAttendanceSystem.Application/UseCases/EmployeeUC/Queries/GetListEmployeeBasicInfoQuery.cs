using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Queries
{
    public record GetListEmployeeBasicInfoQuery();

    public record GetListEmployeeBasicInfoResponse(
        List<GetListEmployeeBasicInfoResponseData> Data
    );

    public record GetListEmployeeBasicInfoResponseData(
        Guid Id,
        string FullName,
        string Code
    );
}
