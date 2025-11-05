using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.WorkTime.Commands;

public record UpdateWorkTimeCommand(
    Guid Id,
    string Name,
    TimeOnly ValidCheckInTime,
    TimeOnly ValidCheckOutTime,
    bool IsActive
);

public class UpdateWorkTimeCommandHandler
{
    private readonly IWorkTimeRepository _workTimeRepository;
    private readonly ILogger<UpdateWorkTimeCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateWorkTimeCommandHandler(
        IWorkTimeRepository workTimeRepository,
        ILogger<UpdateWorkTimeCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _workTimeRepository = workTimeRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result> ExecuteAsync(UpdateWorkTimeCommand command)
    {
        try
        {
            var entity = await _workTimeRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }

            command.Adapt(entity);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = _currentUserService.GetCurrentUserId();

            await _workTimeRepository.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình cập nhật ca làm việc");
            return Result.Fail(new InternalError());
        }
    }
}
