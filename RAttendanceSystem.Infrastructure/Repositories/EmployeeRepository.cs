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
    }
}
