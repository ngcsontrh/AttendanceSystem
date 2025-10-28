using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Application.Features.LeaveRequest.DTOs;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;

namespace AttendanceSystem.Application.Features.LeaveRequest.Commands;

public record CreateLeaveRequestCommand(
    Guid EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    string Reason,
    Guid ApprovedById
);

public class CreateLeaveRequestCommandHandler
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILogger<CreateLeaveRequestCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public CreateLeaveRequestCommandHandler(
        ILeaveRequestRepository leaveRequestRepository,
        ILogger<CreateLeaveRequestCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result<LeaveRequestDTO>> ExecuteAsync(CreateLeaveRequestCommand command)
    {
        try
        {
            var entity = command.Adapt<Domain.Entities.LeaveRequest>();
            entity.Id = Guid.CreateVersion7();
            entity.CreatedAt = DateTime.Now;
            entity.CreatedById = _currentUserService.GetCurrentUserId();
            entity.Status = LeaveStatus.Pending;

            await _leaveRequestRepository.AddAsync(entity);

            return Result.Ok(entity.Adapt<LeaveRequestDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình tạo đơn xin nghỉ");
            return Result.Fail<LeaveRequestDTO>(new InternalError());
        }
    }
}
