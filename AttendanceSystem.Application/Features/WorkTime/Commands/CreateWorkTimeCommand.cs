using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Application.Features.WorkTime.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;

namespace AttendanceSystem.Application.Features.WorkTime.Commands;

public record CreateWorkTimeCommand(
    string Name,
    TimeOnly ValidCheckInTime,
    TimeOnly ValidCheckOutTime,
    bool IsActive
);

public class CreateWorkTimeCommandHandler
{
    private readonly IWorkTimeRepository _workTimeRepository;
    private readonly ILogger<CreateWorkTimeCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkTimeCommandHandler(
        IWorkTimeRepository workTimeRepository,
        ILogger<CreateWorkTimeCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _workTimeRepository = workTimeRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<WorkTimeDTO>> ExecuteAsync(CreateWorkTimeCommand command)
    {
        try
        {
            if (command.IsActive)
            {
                var activeWorkTime = await _workTimeRepository.GetActiveWorkTimeAsync();
                if (activeWorkTime != null)
                {
                    return Result.Fail<WorkTimeDTO>(new BusinessError("Đã có ca làm việc đang hoạt động. Vui lòng vô hiệu hóa ca làm việc hiện tại trước khi tạo ca làm việc mới."));
                }
            }

            var entity = command.Adapt<Domain.Entities.WorkTime>();
            entity.Id = Guid.CreateVersion7();
            entity.CreatedAt = DateTime.Now;
            entity.CreatedById = _currentUserService.GetCurrentUserId();

            await _workTimeRepository.AddAsync(entity);

            return Result.Ok(entity.Adapt<WorkTimeDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình tạo ca làm việc");
            return Result.Fail<WorkTimeDTO>(new InternalError());
        }
    }
}
