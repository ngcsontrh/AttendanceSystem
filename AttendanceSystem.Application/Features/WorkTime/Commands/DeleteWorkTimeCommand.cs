using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.WorkTime.Commands;

public record DeleteWorkTimeCommand(
    Guid Id
);

public class DeleteWorkTimeCommandHandler
{
    private readonly IWorkTimeRepository _workTimeRepository;
    private readonly ILogger<DeleteWorkTimeCommandHandler> _logger;

    public DeleteWorkTimeCommandHandler(
        IWorkTimeRepository workTimeRepository,
        ILogger<DeleteWorkTimeCommandHandler> logger)
    {
        _workTimeRepository = workTimeRepository;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(DeleteWorkTimeCommand command)
    {
        try
        {
            var entity = await _workTimeRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }

            await _workTimeRepository.DeleteAsync(entity);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình xóa ca làm việc");
            return Result.Fail(new InternalError());
        }
    }
}
