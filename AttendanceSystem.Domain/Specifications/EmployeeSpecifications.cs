using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Specifications;

public class GetPageEmployeeSpecification : Specification<Employee>
{
    public GetPageEmployeeSpecification(
        string? code,
        string? fullName,
        string? email,
        Guid? departmentId)
    {
        if (code != null)
        {
            Query.Where(e => e.Code.Contains(code));
        }
        if (fullName != null)
        {
            Query.Where(e => e.FullName.Contains(fullName));
        }
        if (email != null)
        {
            Query.Where(e => e.Email.Contains(email));
        }
        if (departmentId.HasValue)
        {
            Query.Where(e => e.DepartmentId == departmentId.Value);
        }
        Query.OrderByDescending(e => e.CreatedAt);
    }
}

public class GetEmployeeByUserIdSpecification : Specification<Employee>
{
    public GetEmployeeByUserIdSpecification(Guid userId)
    {
        Query.Where(e => e.UserId == userId);
    }
}