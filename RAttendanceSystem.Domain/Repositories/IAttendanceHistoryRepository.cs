using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Repositories
{
    public interface IAttendanceHistoryRepository : IRepositoryBase<AttendanceHistory>
    {
        new Task<(IReadOnlyList<AttendanceHistory>, int)> GetPageAsync(ISpecification<AttendanceHistory> specification);
    }
}