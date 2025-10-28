using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure.Repositories;

public class AttendanceHistoryRepository : RepositoryBase<AttendanceHistory>, IAttendanceHistoryRepository
{
    public AttendanceHistoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<AttendanceHistory>, int)> GetPageAsync(ISpecification<AttendanceHistory> specification, int pageIndex, int pageSize)
    {
        var totalCount = await CountAsync(specification);
        var query = SpecificationEvaluator.GetQuery(DbContext.Set<AttendanceHistory>(), specification);
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        return (items, totalCount);
    }
}
