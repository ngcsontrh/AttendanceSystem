using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Application.Features.AttendanceHistory.DTOs;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;

namespace AttendanceSystem.Application.Features.AttendanceHistory.Commands;

public record CreateAttendanceHistoryCommand(
    AttendanceTypeDTO Type,
    AttendanceStatusDTO Status
);

public class CreateAttendanceHistoryCommandHandler
{
    private readonly IAttendanceCodeRepository _attendanceCodeRepository;
    private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
    private readonly ILogger<CreateAttendanceHistoryCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public CreateAttendanceHistoryCommandHandler(
        IAttendanceCodeRepository attendanceCodeRepository,
        IAttendanceHistoryRepository attendanceHistoryRepository,
        ILogger<CreateAttendanceHistoryCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _attendanceCodeRepository = attendanceCodeRepository;
        _attendanceHistoryRepository = attendanceHistoryRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<AttendanceHistoryDTO>> ExecuteAsync(CreateAttendanceHistoryCommand command)
    {
        try
        {
            var attendanceTime = DateTime.Now;            
            var attendanceType = command.Type.Adapt<AttendanceType>();
            var attendanceStatus = command.Status.Adapt<AttendanceStatus>();

            var entity = new Domain.Entities.AttendanceHistory
            {
                Id = Guid.CreateVersion7(),
                AttendanceDate = attendanceTime,
                CreatedAt = attendanceTime,
                CreatedById = _currentUserService.GetCurrentUserId(),
                EmployeeId = _currentUserService.GetCurrentEmployeeId(),
                Type = attendanceType,
                Status = attendanceStatus
            };

            await _attendanceHistoryRepository.AddAsync(entity);

            return Result.Ok(entity.Adapt<AttendanceHistoryDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình tạo lịch sử điểm danh");
            return Result.Fail<AttendanceHistoryDTO>(new InternalError());
        }
    }
}
