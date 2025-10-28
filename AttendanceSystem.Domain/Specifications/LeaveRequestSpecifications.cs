using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Specifications;

public class GetPageLeaveRequestSpecification : Specification<LeaveRequest>
{
    public GetPageLeaveRequestSpecification(
        Guid? employeeId,
        LeaveStatus? status)
    {
        if (employeeId.HasValue)
        {
            Query.Where(x => x.EmployeeId == employeeId.Value);
        }
        if (status.HasValue)
        {
            Query.Where(x => x.Status == status.Value);
        }
        Query.OrderByDescending(x => x.CreatedAt);
    }
}
