using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Repositories;

public interface IAttendanceHistoryRepository : IRepositoryBase<AttendanceHistory>
{
    Task<(List<AttendanceHistory>, int)> GetPageAsync(
        ISpecification<AttendanceHistory> specification,
        int pageIndex,
        int pageSize);
}
