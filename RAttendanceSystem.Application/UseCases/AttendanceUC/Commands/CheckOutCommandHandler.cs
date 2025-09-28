using RAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceUC.Commands
{
    public class CheckOutCommandHandler
    {
        private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
        private readonly IAttendanceWiFiRepository _attendanceWiFiRepository;
        private readonly ILogger<CheckOutCommandHandler> _logger;

        public CheckOutCommandHandler(
            IAttendanceHistoryRepository attendanceHistoryRepository,
            IAttendanceWiFiRepository attendanceWiFiRepository,
            ILogger<CheckOutCommandHandler> logger)
        {
            _attendanceHistoryRepository = attendanceHistoryRepository;
            _attendanceWiFiRepository = attendanceWiFiRepository;
            _logger = logger;
        }

        public async Task<AttendanceResponse> HandleAsync(CheckOutCommand command)
        {
            try
            {
                var allowedWiFi = await _attendanceWiFiRepository.GetRecordAsync(x =>
                    x.SSID == command.SSID && x.BSSID == command.BSSID);
                if (allowedWiFi == null)
                {
                    throw new InvalidOperationException("WiFi network is not allowed for attendance.");
                }

                var attendance = await _attendanceHistoryRepository.GetRecordAsync(x =>
                    x.EmployeeId == command.EmployeeId &&
                    x.CheckInTime.Date == DateTime.Now.Date);
                if (attendance == null)
                {
                    throw new InvalidOperationException("No check-in record found for today.");
                }

                var checkOutTime = DateTime.Now;
                var checkOutStatus = checkOutTime switch
                {
                    var t when TimeOnly.FromDateTime(t) < allowedWiFi.ValidCheckOutTime => AttendanceStatus.Early,
                    var t when TimeOnly.FromDateTime(t) == allowedWiFi.ValidCheckOutTime => AttendanceStatus.OnTime,
                    _ => AttendanceStatus.Late
                };

                attendance.CheckOutTime = checkOutTime;
                attendance.CheckOutStatus = checkOutStatus;
                await _attendanceHistoryRepository.SaveChangesAsync();

                _logger.LogInformation("Employee {EmployeeId} checked out successfully", command.EmployeeId);

                return new AttendanceResponse(
                    attendance.Id,
                    attendance.CheckOutStatus.Value,
                    attendance.CheckOutTime.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking in employee {EmployeeId}", command.EmployeeId);
                throw;
            }
        }
    }
}