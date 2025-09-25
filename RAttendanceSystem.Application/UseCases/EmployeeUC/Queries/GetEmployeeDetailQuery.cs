using RAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Queries
{
    public record GetEmployeeDetailQuery(Guid Id);

    public record GetEmployeeDetailResponse(
        Guid Id,
        string Code,
        string FullName,
        Gender? Gender,
        DateTime? BirthDate,
        Guid? DepartmentId,
        Guid? TitleId,
        DateTime CreatedAt,
        GetEmployeeDetailResponseTitleData? Title = null,
        GetEmployeeDetailResponseDepartmentData? Department = null
    );

    public record GetEmployeeDetailResponseTitleData(
        Guid Id,
        string Name
    );

    public record GetEmployeeDetailResponseDepartmentData(
        Guid Id,
        string Name
    );
}
