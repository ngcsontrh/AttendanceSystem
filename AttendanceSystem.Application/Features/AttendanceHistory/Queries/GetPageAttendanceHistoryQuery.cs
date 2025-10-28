using AttendanceSystem.Application.Commons.Models;
using AttendanceSystem.Application.Features.AttendanceHistory.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Domain.Specifications;

namespace AttendanceSystem.Application.Features.AttendanceHistory.Queries;

public record GetPageAttendanceHistoryQuery(
    int PageIndex = 1,
    int PageSize = 20,
    Guid? EmployeeId = null,
    DateTime? StartDate = null,
    DateTime? EndDate = null
);

public class GetPageAttendanceHistoryQueryHandler
{
    private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
    private readonly ILogger<GetPageAttendanceHistoryQueryHandler> _logger;

    public GetPageAttendanceHistoryQueryHandler(
        IAttendanceHistoryRepository attendanceHistoryRepository,
        ILogger<GetPageAttendanceHistoryQueryHandler> logger)
    {
        _attendanceHistoryRepository = attendanceHistoryRepository;
        _logger = logger;
    }

    public async Task<Result<PageData<AttendanceHistoryDTO>>> ExecuteAsync(GetPageAttendanceHistoryQuery request)
    {
        try
        {
            var specification = new GetPageAttendanceHistorySpecification(request.EmployeeId, request.StartDate, request.EndDate);
            var (items, totalCount) = await _attendanceHistoryRepository.GetPageAsync(specification, request.PageIndex, request.PageSize);
            return Result.Ok(new PageData<AttendanceHistoryDTO>
            {
                Items = items.Adapt<List<AttendanceHistoryDTO>>(),
                TotalCount = totalCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy danh sách lịch sử điểm danh");
            return Result.Fail<PageData<AttendanceHistoryDTO>>(new InternalError());
        }
    }
}
