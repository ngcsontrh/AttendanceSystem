using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Specifications;

namespace AttendanceSystem.Domain.Repositories;

public interface IAttendanceHistoryRepository : IRepositoryBase<AttendanceHistory>
{
    Task<(List<AttendanceHistory>, int)> GetPageAsync(
        ISpecification<AttendanceHistory> specification,
        int pageIndex,
        int pageSize);
    Task<bool> IsAlreadyCheckedInAsync(IsAlreadyCheckedInSpecification spec);
    Task<bool> IsAlreadyCheckedOutAsync(IsAlreadyCheckedOutSpecification spec);
}
