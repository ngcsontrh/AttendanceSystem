using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application.Commons;
public class AppConstraint
{
    public const string EmployeeIdentifier = "EmployeeIdentifier";
    public const string EmployeeName = "EmployeeName";

    public const string AdminRole = "Admin";
    public const string ManagerRole = "Manager";
    public const string StaffRole = "Staff";

    public const string AdminPolicy = "AdminPolicy";
    public const string ManagerPolicy = "ManagerPolicy";
    public const string StaffPolicy = "StaffPolicy";
}
