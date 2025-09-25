using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Repositories
{
    internal class AttendanceWiFiRepository : RepositoryBase<AttendanceWiFi>, IAttendanceWiFiRepository
    {
        public AttendanceWiFiRepository(RAttendanceDbContext context) : base(context)
        {
        }
    }
}
