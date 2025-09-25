using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceUC.Queries
{
    public class GetAttendanceHistoryQueryHandler
    {
        private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
        private readonly ILogger<GetAttendanceHistoryQueryHandler> _logger;

        public GetAttendanceHistoryQueryHandler(
            IAttendanceHistoryRepository attendanceHistoryRepository,
            ILogger<GetAttendanceHistoryQueryHandler> logger)
        {
            _attendanceHistoryRepository = attendanceHistoryRepository;
            _logger = logger;
        }

        public async Task<GetAttendanceHistoryResponse> HandleAsync(GetAttendanceHistoryQuery query)
        {
            try
            {
                var spec = new AttendanceHistoryFilterSpecificationBuilder()
                    .WithEmployeeId(query.EmployeeId)
                    .WithDateRange(query.FromDate, query.ToDate)
                    .WithDay(query.Day)
                    .WithMonth(query.Year, query.Month)
                    .WithOrderByDescending(x => x.CheckInTime)
                    .Build();

                var result = await _attendanceHistoryRepository.GetPageAsync(spec);

                return MapToResponse(result.Item1, result.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting attendance history");
                throw;
            }
        }

        private GetAttendanceHistoryResponse MapToResponse(IReadOnlyList<AttendanceHistory> attendanceHistories, int total)
        {
            var data = attendanceHistories.Select(x => new AttendanceHistoryResponseData(
                Id: x.Id,
                EmployeeId: x.EmployeeId,
                CreatedAt: x.CreatedAt,
                CheckInTime: x.CheckInTime,
                CheckOutTime: x.CheckOutTime,
                CheckInStatus: x.CheckInStatus,
                CheckOutStatus: x.CheckOutStatus
            )).ToList();

            return new GetAttendanceHistoryResponse(data, total);
        }
    }
}