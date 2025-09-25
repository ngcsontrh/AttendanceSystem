using RAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Queries
{
    public record GetPageEmployeeQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Code = null,
        string? FullName = null,
        Guid? DepartmentId = null,
        Guid? TitleId = null);    

    public record GetPageEmployeeResponse(        
        List<GetPageEmployeeResponseData> Data,
        int TotalRecords);

    public record GetPageEmployeeResponseData(
        Guid Id,
        string FullName,
        string Code,
        Gender? Gender = null,
        DateTime? BirthDate = null,
        Guid? DepartmentId = null,
        Guid? TitleId = null
    );
}
