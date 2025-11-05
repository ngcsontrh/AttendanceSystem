using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations
{
    [Migration(202511050241)]
    public class _202511050241_ChangeLeaveRequestsColumnType : Migration
    {
        public override void Up()
        {
            Alter.Column("ApprovedById")
                .OnTable("LeaveRequests")
                .AsGuid()
                .Nullable();
        }
        public override void Down()
        {
            Alter.Column("ApprovedById")
                .OnTable("LeaveRequests")
                .AsGuid()
                .NotNullable();
        }
    }
}
