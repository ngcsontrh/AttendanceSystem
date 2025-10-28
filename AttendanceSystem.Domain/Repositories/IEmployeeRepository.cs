using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Repositories;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
    Task<(List<Employee>, int)> GetPageAsync(
        ISpecification<Employee> specification,
        int pageIndex,
        int pageSize);
    Task<Employee?> GetByUserIdAsync(Guid userId);
    Task UpdateUserIdAsync(Employee employee, Guid userId);
    Task<bool> CheckEmployeeExistsByDepartmentIdAsync(Guid departmentId);
}
