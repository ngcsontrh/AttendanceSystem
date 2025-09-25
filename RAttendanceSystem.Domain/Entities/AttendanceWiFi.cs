using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Entities
{
    public class AttendanceWiFi
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = null!;
        public string SSID { get; set; } = null!;
        public string BSSID { get; set; } = null!;
        public DateTime ValidCheckInTime { get; set; }
        public DateTime ValidCheckOutTime { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
