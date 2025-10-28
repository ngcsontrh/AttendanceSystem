using AttendanceSystem.Application.Features.LeaveRequest.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;


namespace AttendanceSystem.Application.Features.LeaveRequest.Queries;

public record GetLeaveRequestByIdQuery(
    Guid Id
);

public class GetLeaveRequestByIdQueryHandler
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILogger<GetLeaveRequestByIdQueryHandler> _logger;

    public GetLeaveRequestByIdQueryHandler(
        ILeaveRequestRepository leaveRequestRepository,
        ILogger<GetLeaveRequestByIdQueryHandler> logger)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _logger = logger;
    }

    public async Task<Result<LeaveRequestDTO>> ExecuteAsync(GetLeaveRequestByIdQuery query)
    {
        try
        {
            var entity = await _leaveRequestRepository.GetByIdAsync(query.Id);
            if (entity == null)
            {
                return Result.Fail<LeaveRequestDTO>(new NotFoundError());
            }

            return Result.Ok(entity.Adapt<LeaveRequestDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy thông tin đơn xin nghỉ");
            return Result.Fail<LeaveRequestDTO>(new InternalError());
        }
    }
}
