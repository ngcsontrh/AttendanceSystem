using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.DepartmentUC.Commands
{
    public record CreateDepartmentCommand(
        string Name,
        string? Description = null);

    public record CreateDepartmentResponse(Guid Id);
}
