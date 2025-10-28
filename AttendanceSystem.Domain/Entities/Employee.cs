using AttendanceSystem.Domain.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Entities;
public class Employee : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public Guid DepartmentId { get; set; }
    public EmployeeStatus Status { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid? UserId { get; set; }
}

public enum EmployeeStatus
{
    Active = 0,
    Inactive = 1,
    Suspended = 2
}

public enum Gender
{
    Female = 0,
    Male = 1
}