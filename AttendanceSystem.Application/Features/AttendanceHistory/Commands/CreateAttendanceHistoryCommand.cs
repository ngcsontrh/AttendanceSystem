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
    AttendanceStatusDTO Status,
    Guid WorkTimeId
);

public class CreateAttendanceHistoryCommandHandler
{
    private readonly IAttendanceCodeRepository _attendanceCodeRepository;
    private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
    private readonly IWorkTimeRepository _workTimeRepository;
    private readonly ILogger<CreateAttendanceHistoryCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public CreateAttendanceHistoryCommandHandler(
        IAttendanceCodeRepository attendanceCodeRepository,
        IAttendanceHistoryRepository attendanceHistoryRepository,
        IWorkTimeRepository workTimeRepository,
        ILogger<CreateAttendanceHistoryCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _attendanceCodeRepository = attendanceCodeRepository;
        _attendanceHistoryRepository = attendanceHistoryRepository;
        _workTimeRepository = workTimeRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<AttendanceHistoryDTO>> ExecuteAsync(CreateAttendanceHistoryCommand command)
    {
        try
        {
            var isWorkTimeExist = await _workTimeRepository.IsWorkTimeActiveExistingAsync(command.WorkTimeId);
            if (isWorkTimeExist == false)
            {
                return Result.Fail<AttendanceHistoryDTO>(new BusinessError("Ca làm việc không tồn tại hoặc không hoạt động"));
            }
            var employeeId = _currentUserService.GetCurrentEmployeeId();
            var userId = _currentUserService.GetCurrentUserId();
            var attendanceTime = DateTime.Now;
            if (command.Type == AttendanceTypeDTO.CheckIn)
            {
                var hasCheckedIn = await _attendanceHistoryRepository.IsAlreadyCheckedInAsync(new Domain.Specifications.IsAlreadyCheckedInSpecification(employeeId, attendanceTime));
                if (hasCheckedIn)
                {
                    return Result.Fail<AttendanceHistoryDTO>(new BusinessError("Nhân viên đã điểm danh vào trong ca làm việc này"));
                }
            }
            else if (command.Type == AttendanceTypeDTO.CheckOut)
            {
                var hasCheckedOut = await _attendanceHistoryRepository.IsAlreadyCheckedOutAsync(new Domain.Specifications.IsAlreadyCheckedOutSpecification(employeeId, attendanceTime));
                if (hasCheckedOut)
                {
                    return Result.Fail<AttendanceHistoryDTO>(new BusinessError("Nhân viên đã điểm danh ra trong ca làm việc này"));
                }
            }
            var attendanceType = command.Type.Adapt<AttendanceType>();
            var attendanceStatus = command.Status.Adapt<AttendanceStatus>();

            var entity = new Domain.Entities.AttendanceHistory
            {
                Id = Guid.CreateVersion7(),
                AttendanceDate = attendanceTime,
                CreatedAt = attendanceTime,
                CreatedById = userId,
                EmployeeId = employeeId,
                Type = attendanceType,
                Status = attendanceStatus,
                WorkTimeId = command.WorkTimeId
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
