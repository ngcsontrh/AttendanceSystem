using RAttendanceSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.UseCases.AttendanceUC.Commands
{
    public class CheckInCommandHandler
    {
        private readonly IAttendanceHistoryRepository _attendanceHistoryRepository;
        private readonly IAttendanceWiFiRepository _attendanceWiFiRepository;        
        private readonly ILogger<CheckInCommandHandler> _logger;

        public CheckInCommandHandler(
            IAttendanceHistoryRepository attendanceHistoryRepository,
            IAttendanceWiFiRepository attendanceWiFiRepository,
            ILogger<CheckInCommandHandler> logger)
        {
            _attendanceHistoryRepository = attendanceHistoryRepository;
            _attendanceWiFiRepository = attendanceWiFiRepository;
            _logger = logger;
        }

        public async Task<AttendanceResponse> HandleAsync(CheckInCommand command)
        {
            try
            {
                var allowedWiFi = await _attendanceWiFiRepository.GetRecordAsync(x => 
                    x.SSID == command.SSID && x.BSSID == command.BSSID);                
                if (allowedWiFi == null)
                {
                    throw new InvalidOperationException("WiFi network is not allowed for attendance.");
                }

                var existingAttendance = await _attendanceHistoryRepository.GetRecordAsync(x => 
                    x.EmployeeId == command.EmployeeId && 
                    x.CheckInTime.Date == DateTime.Now.Date);
                if (existingAttendance != null)
                {
                    throw new InvalidOperationException("Already checked in today.");
                }
                
                var checkInTime = DateTime.Now;
                var checkInStatus = checkInTime switch
                {
                    var t when TimeOnly.FromDateTime(t) < allowedWiFi.ValidCheckInTime => AttendanceStatus.Early,
                    var t when TimeOnly.FromDateTime(t) == allowedWiFi.ValidCheckInTime => AttendanceStatus.OnTime,
                    _ => AttendanceStatus.Late
                };
                var attendance = new AttendanceHistory
                {
                    Id = Guid.CreateVersion7(),
                    EmployeeId = command.EmployeeId,
                    CheckInTime = checkInTime,
                    CheckInStatus = checkInStatus,
                    CreatedAt = DateTime.Now,
                };
                _attendanceHistoryRepository.Add(attendance);
                await _attendanceHistoryRepository.SaveChangesAsync();

                _logger.LogInformation("Employee {EmployeeId} checked in successfully", command.EmployeeId);

                return new AttendanceResponse(
                    attendance.Id,
                    attendance.CheckInStatus,
                    attendance.CheckInTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking in employee {EmployeeId}", command.EmployeeId);
                throw;
            }
        }
    }
}