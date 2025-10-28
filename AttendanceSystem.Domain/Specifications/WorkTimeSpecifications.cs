using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Specifications;

public class GetPageWorkTimeSpecification : Specification<WorkTime>
{
    public GetPageWorkTimeSpecification(
        string? name,
        bool? isActive)
    {
        if (name != null)
        {
            Query.Where(x => x.Name.Contains(name));
        }
        if (isActive.HasValue)
        {
            Query.Where(x => x.IsActive == isActive.Value);
        }
        Query.OrderByDescending(x => x.CreatedAt);
    }
}
