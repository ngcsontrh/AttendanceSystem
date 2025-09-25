using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.DepartmentUC.Commands
{
    public record UpdateDepartmentCommand(
        Guid Id,
        string Name,
        string? Description = null);
}
