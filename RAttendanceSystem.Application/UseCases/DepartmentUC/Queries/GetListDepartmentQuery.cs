using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.DepartmentUC.Queries
{
    public record GetListDepartmentQuery();

    public record GetListDepartmentResponse(
        List<GetListDepartmentResponseData> Data
    );

    public record GetListDepartmentResponseData(
        Guid Id,
        string Name,
        string? Description = null);    
}
