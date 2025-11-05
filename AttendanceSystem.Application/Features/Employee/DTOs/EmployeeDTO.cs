using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.Employee.DTOs;
public class EmployeeDTO
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public Guid DepartmentId { get; set; }
    public EmployeeStatus Status { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid? UserId { get; set; }
}
