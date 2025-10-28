using AttendanceSystem.Application.Commons.Models;
using AttendanceSystem.Application.Features.WorkTime.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;
using AttendanceSystem.Domain.Specifications;

namespace AttendanceSystem.Application.Features.WorkTime.Queries;

public record GetPageWorkTimeQuery(
    int PageIndex = 1,
    int PageSize = 20,
    string? Name = null,
    bool? IsActive = null
);

public class GetPageWorkTimeQueryHandler
{
    private readonly IWorkTimeRepository _workTimeRepository;
    private readonly ILogger<GetPageWorkTimeQueryHandler> _logger;

    public GetPageWorkTimeQueryHandler(
        IWorkTimeRepository workTimeRepository,
        ILogger<GetPageWorkTimeQueryHandler> logger)
    {
        _workTimeRepository = workTimeRepository;
        _logger = logger;
    }

    public async Task<Result<PageData<WorkTimeDTO>>> ExecuteAsync(GetPageWorkTimeQuery request)
    {
        try
        {
            var specification = new GetPageWorkTimeSpecification(request.Name, request.IsActive);
            var (items, totalCount) = await _workTimeRepository.GetPageAsync(specification, request.PageIndex, request.PageSize);
            return Result.Ok(new PageData<WorkTimeDTO>
            {
                Items = items.Adapt<List<WorkTimeDTO>>(),
                TotalCount = totalCount
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy danh sách ca làm việc");
            return Result.Fail<PageData<WorkTimeDTO>>(new InternalError());
        }
    }
}
