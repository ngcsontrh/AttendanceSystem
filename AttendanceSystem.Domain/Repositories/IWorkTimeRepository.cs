using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Repositories;

public interface IWorkTimeRepository : IRepositoryBase<WorkTime>
{
    Task<(List<WorkTime>, int)> GetPageAsync(
        ISpecification<WorkTime> specification,
        int pageIndex,
        int pageSize);

    Task<WorkTime?> GetActiveWorkTimeAsync();
}
