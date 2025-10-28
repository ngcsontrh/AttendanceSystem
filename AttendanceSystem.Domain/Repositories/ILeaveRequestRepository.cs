using Ardalis.Specification;
using AttendanceSystem.Domain.Entities;

namespace AttendanceSystem.Domain.Repositories;

public interface ILeaveRequestRepository : IRepositoryBase<LeaveRequest>
{
    Task<(List<LeaveRequest>, int)> GetPageAsync(
        ISpecification<LeaveRequest> specification,
        int pageIndex,
        int pageSize);
}
