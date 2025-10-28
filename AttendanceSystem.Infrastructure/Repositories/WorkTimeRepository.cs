using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure.Repositories;

public class WorkTimeRepository : RepositoryBase<WorkTime>, IWorkTimeRepository
{
    public WorkTimeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<WorkTime?> GetActiveWorkTimeAsync()
    {
        var entity = await DbContext.Set<WorkTime>()
            .FirstOrDefaultAsync(x => x.IsActive);
        return entity;
    }

    public async Task<(List<WorkTime>, int)> GetPageAsync(ISpecification<WorkTime> specification, int pageIndex, int pageSize)
    {
        var totalCount = await CountAsync(specification);
        var query = SpecificationEvaluator.GetQuery(DbContext.Set<WorkTime>(), specification);
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        return (items, totalCount);
    }
}
