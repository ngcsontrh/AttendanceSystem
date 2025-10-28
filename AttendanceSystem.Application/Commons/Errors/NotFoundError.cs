using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Errors;
public class NotFoundError : Error
{
    public NotFoundError(string message = "Dữ liệu không tồn tại") : base(message)
    {
    }
}
