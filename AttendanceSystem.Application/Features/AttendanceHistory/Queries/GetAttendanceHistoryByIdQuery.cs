using AttendanceSystem.Application.Features.AttendanceHistory.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.AttendanceHistory.Queries;

public record GetAttendanceHistoryByIdQuery(
    Guid Id
);

public class GetAttendanceHistoryByIdQueryHandler
{
    private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
    private readonly ILogger<GetAttendanceHistoryByIdQueryHandler> _logger;

    public GetAttendanceHistoryByIdQueryHandler(
        IAttendanceHistoryRepository attendanceHistoryRepository,
        ILogger<GetAttendanceHistoryByIdQueryHandler> logger)
    {
        _attendanceHistoryRepository = attendanceHistoryRepository;
        _logger = logger;
    }

    public async Task<Result<AttendanceHistoryDTO>> ExecuteAsync(GetAttendanceHistoryByIdQuery query)
    {
        try
        {
            var entity = await _attendanceHistoryRepository.GetByIdAsync(query.Id);
            if (entity == null)
            {
                return Result.Fail<AttendanceHistoryDTO>(new NotFoundError());
            }

            return Result.Ok(entity.Adapt<AttendanceHistoryDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy thông tin lịch sử điểm danh");
            return Result.Fail<AttendanceHistoryDTO>(new InternalError());
        }
    }
}
