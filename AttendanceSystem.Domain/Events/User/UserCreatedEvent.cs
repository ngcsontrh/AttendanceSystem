using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Domain.Events.User
{
    public record UserCreatedEvent(Guid UserId, string UserName, string UserEmail);
}
