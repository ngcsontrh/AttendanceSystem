using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.AttendanceHistory.Commands;

public record DeleteAttendanceHistoryCommand(
    Guid Id
);

public class DeleteAttendanceHistoryCommandHandler
{
    private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
    private readonly ILogger<DeleteAttendanceHistoryCommandHandler> _logger;

    public DeleteAttendanceHistoryCommandHandler(
        IAttendanceHistoryRepository attendanceHistoryRepository,
        ILogger<DeleteAttendanceHistoryCommandHandler> logger)
    {
        _attendanceHistoryRepository = attendanceHistoryRepository;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(DeleteAttendanceHistoryCommand command)
    {
        try
        {
            var entity = await _attendanceHistoryRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }

            await _attendanceHistoryRepository.DeleteAsync(entity);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình xóa lịch sử điểm danh");
            return Result.Fail(new InternalError());
        }
    }
}
