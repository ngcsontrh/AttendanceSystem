using AttendanceSystem.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Entities;
public class WorkTime : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public TimeOnly ValidCheckInTime { get; set; }
    public TimeOnly ValidCheckOutTime { get; set; }
    public bool IsActive { get; set; }
}
