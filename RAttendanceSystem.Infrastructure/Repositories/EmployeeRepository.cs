using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Repositories
{
    internal class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RAttendanceDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Employee>> GetListBasicInfoAsync()
        {
            var result = await _context.Employees
                .Select(e => new Employee
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    Code = e.Code
                })
                .AsNoTracking()
                .ToListAsync();
            return result;
        }
    }
}
