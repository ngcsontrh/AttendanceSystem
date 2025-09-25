using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.TitleUC.Commands
{
    public record CreateTitleCommand(
        string Name,
        string? Description = null);

    public record CreateTitleResponse(Guid Id);
}
