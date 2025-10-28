using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Errors;
public class InternalError : Error
{
    public InternalError(string message = "Đã xảy ra lỗi trong quá trình xử lý") : base(message)
    {
    }
}
