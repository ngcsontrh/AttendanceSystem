using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.AttendanceHistory.Commands;

public record UpdateAttendanceHistoryCommand(
    Guid Id,
    Guid EmployeeId,
    DateTime AttendanceDate,
    AttendanceType Type,
    AttendanceStatus Status
);

public class UpdateAttendanceHistoryCommandHandler
{
    private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
    private readonly ILogger<UpdateAttendanceHistoryCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateAttendanceHistoryCommandHandler(
        IAttendanceHistoryRepository attendanceHistoryRepository,
        ILogger<UpdateAttendanceHistoryCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _attendanceHistoryRepository = attendanceHistoryRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result> ExecuteAsync(UpdateAttendanceHistoryCommand command)
    {
        try
        {
            var entity = await _attendanceHistoryRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }

            command.Adapt(entity);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = _currentUserService.GetCurrentUserId();

            await _attendanceHistoryRepository.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình cập nhật lịch sử điểm danh");
            return Result.Fail(new InternalError());
        }
    }
}
