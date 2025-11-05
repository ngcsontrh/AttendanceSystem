using AttendanceSystem.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Entities;
public class AttendanceHistory : EntityBase
{
    public Guid EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public AttendanceType Type { get; set; }
    public AttendanceStatus Status { get; set; }
    public Guid WorkTimeId { get; set; }
}

public enum AttendanceType
{
    CheckIn,
    CheckOut
}

public enum AttendanceStatus
{
    OnTime,
    Late,
    Early
}