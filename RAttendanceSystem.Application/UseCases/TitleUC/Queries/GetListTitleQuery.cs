using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.TitleUC.Queries
{
    public record GetListTitleResponse(
        List<GetListTitleResponseData> Data);

    public record GetListTitleResponseData(
        Guid Id,
        string Name,
        string? Description = null);    
}
