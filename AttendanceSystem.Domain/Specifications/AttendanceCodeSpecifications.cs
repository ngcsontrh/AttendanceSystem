using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Specifications;

public class GetPageAttendanceCodeSpecification : Specification<AttendanceCode>
{
    public GetPageAttendanceCodeSpecification()
    {
        Query.OrderByDescending(x => x.CreatedAt);
    }
}
