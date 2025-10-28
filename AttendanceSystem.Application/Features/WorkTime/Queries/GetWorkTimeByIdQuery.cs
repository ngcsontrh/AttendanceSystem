using AttendanceSystem.Application.Features.WorkTime.DTOs;
using AttendanceSystem.Domain.Repositories;
using FluentResults;
using Mapster;
using Microsoft.Extensions.Logging;
using AttendanceSystem.Application.Commons.Errors;

namespace AttendanceSystem.Application.Features.WorkTime.Queries;

public record GetWorkTimeByIdQuery(
    Guid Id
);

public class GetWorkTimeByIdQueryHandler
{
    private readonly IWorkTimeRepository _workTimeRepository;
    private readonly ILogger<GetWorkTimeByIdQueryHandler> _logger;

    public GetWorkTimeByIdQueryHandler(
        IWorkTimeRepository workTimeRepository,
        ILogger<GetWorkTimeByIdQueryHandler> logger)
    {
        _workTimeRepository = workTimeRepository;
        _logger = logger;
    }

    public async Task<Result<WorkTimeDTO>> ExecuteAsync(GetWorkTimeByIdQuery query)
    {
        try
        {
            var entity = await _workTimeRepository.GetByIdAsync(query.Id);
            if (entity == null)
            {
                return Result.Fail<WorkTimeDTO>(new NotFoundError());
            }

            return Result.Ok(entity.Adapt<WorkTimeDTO>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình lấy thông tin ca làm việc");
            return Result.Fail<WorkTimeDTO>(new InternalError());
        }
    }
}
