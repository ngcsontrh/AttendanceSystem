using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Repositories;

public interface IAttendanceCodeRepository : IRepositoryBase<AttendanceCode>
{
    Task<(List<AttendanceCode>, int)> GetPageAsync(
        ISpecification<AttendanceCode> specification,
        int pageIndex,
        int pageSize);

    Task<bool> IsValidCodeAsync(string code, DateTime attendanceDate);
    Task<AttendanceCode?> GetByCodeAsync(string code, DateTime today);
}
