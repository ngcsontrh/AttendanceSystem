using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Entities
{
    public class DeviceToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public string Platform { get; set; } = null!;
        public Guid? EmployeeId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Employee? Employee { get; set; }
    }
}
