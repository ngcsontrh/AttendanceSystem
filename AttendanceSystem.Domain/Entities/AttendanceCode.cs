using AttendanceSystem.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Entities;
public class AttendanceCode : EntityBase
{
    public string? Code { get; set; }
    public DateTime ExpirationAt { get; set; }
}