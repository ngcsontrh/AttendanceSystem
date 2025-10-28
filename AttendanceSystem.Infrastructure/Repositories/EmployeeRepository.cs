using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using AttendanceSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Infrastructure.Repositories;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CheckEmployeeExistsByDepartmentIdAsync(Guid departmentId)
    {
        var exists = await DbContext.Set<Employee>().AnyAsync(x => x.DepartmentId == departmentId);
        return exists;
    }

    public async Task<Employee?> GetByUserIdAsync(Guid userId)
    {
        var employee = await DbContext.Set<Employee>().FirstOrDefaultAsync(x => x.UserId == userId);
        return employee;
    }

    public async Task<(List<Employee>, int)> GetPageAsync(ISpecification<Employee> specification, int pageIndex, int pageSize)
    {
        var totalCount = await CountAsync(specification);
        var query = SpecificationEvaluator.GetQuery(DbContext.Set<Employee>(), specification);
        var items = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task UpdateUserIdAsync(Employee employee, Guid userId)
    {
        employee.UserId = userId;
        await DbContext.SaveChangesAsync();
    }
}
