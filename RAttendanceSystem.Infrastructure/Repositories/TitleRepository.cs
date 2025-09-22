using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Repositories
{
    internal class TitleRepository : RepositoryBase<Title>, ITitleRepository
    {
        public TitleRepository(RAttendanceDbContext context) : base(context)
        {
        }
    }
}
