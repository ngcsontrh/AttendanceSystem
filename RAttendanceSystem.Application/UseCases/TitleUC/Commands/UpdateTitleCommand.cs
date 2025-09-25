using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.TitleUC.Commands
{
    public record UpdateTitleCommand(
        Guid Id,
        string Name,
        string? Description = null);
}
