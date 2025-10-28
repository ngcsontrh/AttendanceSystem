using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure.Repositories;

public class AttendanceCodeRepository : RepositoryBase<AttendanceCode>, IAttendanceCodeRepository
{
    public AttendanceCodeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsValidCodeAsync(string code, DateTime attendanceDate)
    {
        var result = await DbContext.Set<AttendanceCode>()
            .Where(ac => ac.Code == code && ac.ExpirationAt >= attendanceDate)
            .AnyAsync();
        return result;
    }

    public async Task<(List<AttendanceCode>, int)> GetPageAsync(ISpecification<AttendanceCode> specification, int pageIndex, int pageSize)
    {
        var totalCount = await CountAsync(specification);
        var query = SpecificationEvaluator.GetQuery(DbContext.Set<AttendanceCode>(), specification);
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<AttendanceCode?> GetByCodeAsync(string code, DateTime today)
    {
        var result = await DbContext.Set<AttendanceCode>()
            .FirstOrDefaultAsync(ac => ac.Code == code && ac.ExpirationAt.Date == today.Date);
        return result;
    }
}
