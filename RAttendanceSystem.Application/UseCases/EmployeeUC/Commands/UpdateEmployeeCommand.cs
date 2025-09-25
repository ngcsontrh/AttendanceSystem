using RAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.EmployeeUC.Commands
{
    public record UpdateEmployeeCommand(
        Guid Id,
        string FullName,
        string Code,
        Gender? Gender = null,
        DateTime? BirthDate = null,
        Guid? DepartmentId = null,
        Guid? TitleId = null);
}
