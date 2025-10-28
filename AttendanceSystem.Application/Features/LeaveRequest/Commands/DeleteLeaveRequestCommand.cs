using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.LeaveRequest.Commands;

public record DeleteLeaveRequestCommand(
    Guid Id
);

public class DeleteLeaveRequestCommandHandler
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILogger<DeleteLeaveRequestCommandHandler> _logger;

    public DeleteLeaveRequestCommandHandler(
        ILeaveRequestRepository leaveRequestRepository,
        ILogger<DeleteLeaveRequestCommandHandler> logger)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _logger = logger;
    }

    public async Task<Result> ExecuteAsync(DeleteLeaveRequestCommand command)
    {
        try
        {
            var entity = await _leaveRequestRepository.GetByIdAsync(command.Id);
            if (entity == null)
            {
                return Result.Fail(new NotFoundError());
            }

            await _leaveRequestRepository.DeleteAsync(entity);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình xóa đơn xin nghỉ");
            return Result.Fail(new InternalError());
        }
    }
}
