using AttendanceSystem.Application.Commons.Models;
using AttendanceSystem.Application.Features.LeaveRequest.DTOs;
using AttendanceSystem.Domain.Entities;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Domain.Specifications;

namespace AttendanceSystem.Application.Features.LeaveRequest.Queries;

public record GetPageLeaveRequestQuery(
    int PageIndex = 1,
    int PageSize = 20,
    Guid? EmployeeId = null,
    LeaveStatus? Status = null
);

public class GetPageLeaveRequestQueryHandler
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILogger<GetPageLeaveRequestQueryHandler> _logger;

    public GetPageLeaveRequestQueryHandler(
        ILeaveRequestRepository leaveRequestRepository,
        ILogger<GetPageLeaveRequestQueryHandler> logger)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _logger = logger;
    }

    public async Task<Result<PageData<LeaveRequestDTO>>> ExecuteAsync(GetPageLeaveRequestQuery request)
    {
        try
        {
            var specification = new GetPageLeaveRequestSpecification(request.EmployeeId, request.Status);
            var (items, totalCount) = await _leaveRequestRepository.GetPageAsync(specification, request.PageIndex, request.PageSize);
            return Result.Ok(new PageData<LeaveRequestDTO>
            {
                Items = items.Adapt<List<LeaveRequestDTO>>(),
                TotalCount = totalCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy danh sách đơn xin nghỉ");
            return Result.Fail<PageData<LeaveRequestDTO>>(new InternalError());
        }
    }
}
