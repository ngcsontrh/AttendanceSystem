using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Repositories
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<IReadOnlyList<Employee>> GetListBasicInfoAsync();
    }
}
