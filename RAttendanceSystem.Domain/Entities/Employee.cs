using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string? KeycloakId { get; set; }
        public string Code { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public Gender? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? TitleId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Department? Department { get; set; }
        public Title? Title { get; set; }        
    }
}
