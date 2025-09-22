using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Repositories
{
    internal class SystemNotificationRepository : RepositoryBase<SystemNotification>, ISystemNotificationRepository
    {
        public SystemNotificationRepository(RAttendanceDbContext context) : base(context)
        {
        }
    }
}
