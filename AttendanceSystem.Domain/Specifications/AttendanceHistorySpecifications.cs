using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Specifications;

public class GetPageAttendanceHistorySpecification : Specification<AttendanceHistory>
{
    public GetPageAttendanceHistorySpecification(
        Guid? employeeId,
        DateTime? startDate,
        DateTime? endDate)
    {
        if (employeeId.HasValue)
        {
            Query.Where(x => x.EmployeeId == employeeId.Value);
        }
        if (startDate.HasValue)
        {
            Query.Where(x => x.AttendanceDate >= startDate.Value);
        }
        if (endDate.HasValue)
        {
            Query.Where(x => x.AttendanceDate <= endDate.Value);
        }
        Query.OrderByDescending(x => x.AttendanceDate);
    }
}

public class IsAlreadyCheckedInSpecification : Specification<AttendanceHistory>
{
    public IsAlreadyCheckedInSpecification(Guid employeeId, DateTime attendanceDate)
    {
        Query.Where(x => x.EmployeeId == employeeId &&
                         x.AttendanceDate.Date == attendanceDate.Date &&
                         x.Type == AttendanceType.CheckIn);
    }
}

public class IsAlreadyCheckedOutSpecification : Specification<AttendanceHistory>
{
    public IsAlreadyCheckedOutSpecification(Guid employeeId, DateTime attendanceDate)
    {
        Query.Where(x => x.EmployeeId == employeeId &&
                         x.AttendanceDate.Date == attendanceDate.Date &&
                         x.Type == AttendanceType.CheckOut);
    }
}