using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Application.Features.LeaveRequest.DTOs;

public class LeaveRequestDTO
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public LeaveStatus Status { get; set; }
    public Guid? ApprovedById { get; set; }
}
