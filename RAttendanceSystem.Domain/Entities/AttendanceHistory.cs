using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Entities
{
    public class AttendanceHistory
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime CheckInTime { get; set; }
        public AttendanceStatus CheckInStatus { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus? CheckOutStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
