using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Repositories
{
    internal class AttendanceHistoryRepository : RepositoryBase<AttendanceHistory>, IAttendanceHistoryRepository
    {
        public AttendanceHistoryRepository(RAttendanceDbContext context) : base(context)
        {
        }

        public new async Task<(IReadOnlyList<AttendanceHistory>, int)> GetPageAsync(ISpecification<AttendanceHistory> specification)
        {
            var predicate = specification.Criteria;
            var queryable = _context.Set<AttendanceHistory>()
                .Include(x => x.Employee)
                .Where(predicate);
            
            var totalRecords = await queryable.CountAsync();

            if (specification.OrderBy != null)
            {
                queryable = queryable.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                queryable = queryable.OrderByDescending(specification.OrderByDescending);
            }

            if (specification.IsPagingEnabled && specification.Skip.HasValue && specification.Take.HasValue)
            {
                queryable = queryable.Skip(specification.Skip.Value).Take(specification.Take.Value);
            }

            var records = await queryable.ToListAsync();
            return (records, totalRecords);
        }
    }
}