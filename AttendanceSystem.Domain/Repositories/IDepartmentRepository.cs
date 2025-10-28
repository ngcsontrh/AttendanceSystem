using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Repositories;
public interface IDepartmentRepository : IRepositoryBase<Department>
{
    Task<(List<Department>, int)> GetPageAsync(
        ISpecification<Department> specification,
        int pageIndex,
        int pageSize);
}
