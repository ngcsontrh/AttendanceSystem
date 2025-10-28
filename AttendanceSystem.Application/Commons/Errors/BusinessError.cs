using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons.Errors;
public class BusinessError : Error
{
    public BusinessError(string message) : base(message)
    {
    }
}
