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
