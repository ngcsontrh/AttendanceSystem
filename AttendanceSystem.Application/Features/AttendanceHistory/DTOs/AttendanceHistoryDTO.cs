namespace AttendanceSystem.Application.Features.AttendanceHistory.DTOs;

public class AttendanceHistoryDTO
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public AttendanceTypeDTO Type { get; set; }
    public AttendanceStatusDTO Status { get; set; }
    public Guid WorkTimeId { get; set; }
}

public enum AttendanceTypeDTO
{
    CheckIn,
    CheckOut
}

public enum AttendanceStatusDTO
{
    OnTime,
    Late,
    Early
}