using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Entities
{
    public class SystemNotification
    {
        public Guid Id { get; set; }
        public Guid ReceiverId { get; set; }
        public string Title { get; set; } = null!;
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }

        public Employee Receiver { get; set; } = null!;
    }
}
