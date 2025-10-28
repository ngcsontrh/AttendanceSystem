using AttendanceSystem.Application.Commons.Services;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.LeaveRequest.Commands;

public record UpdateLeaveRequestCommand(
    Guid Id,
    Guid EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    string Reason,
    LeaveStatus Status,
    Guid ApprovedById
);

public class UpdateLeaveRequestCommandHandler
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILogger<UpdateLeaveRequestCommandHandler> _logger;
    private readonly ICurrentUserService _currentUserService;

    public UpdateLeaveRequestCommandHandler(
        ILeaveRequestRepository leaveRequestRepository,
        ILogger<UpdateLeaveRequestCommandHandler> logger,
        ICurrentUserService currentUserService)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<Result> ExecuteAsync(UpdateLeaveRequestCommand command)
    {
        try
        {
            var entity = await _leaveRequestRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }

            command.Adapt(entity);
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = _currentUserService.GetCurrentUserId();

            await _leaveRequestRepository.SaveChangesAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình cập nhật đơn xin nghỉ");
            return Result.Fail(new InternalError());
        }
    }
}
