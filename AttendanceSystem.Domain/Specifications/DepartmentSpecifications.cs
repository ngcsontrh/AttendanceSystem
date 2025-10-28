using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Specifications;

public class GetPageDepartmentSpecification : Specification<Department>
{
    public GetPageDepartmentSpecification(
        string? code,
        string? name)
    {
        if (code != null)
        {
            Query.Where(d => d.Code.Contains(code));
        }
        if (name != null)
        {
            Query.Where(d => d.Name.Contains(name));
        }
        Query.OrderByDescending(d => d.CreatedAt);
    }
}