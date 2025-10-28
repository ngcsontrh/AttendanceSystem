namespace AttendanceSystem.Application.Features.WorkTime.DTOs;

public class WorkTimeDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeOnly ValidCheckInTime { get; set; }
    public TimeOnly ValidCheckOutTime { get; set; }
    public bool IsActive { get; set; }
}
