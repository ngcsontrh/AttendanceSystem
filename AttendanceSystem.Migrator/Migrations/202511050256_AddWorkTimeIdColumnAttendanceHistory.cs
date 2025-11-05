using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations
{
    [Migration(202511050256)]
    public class _202511050256_AddWorkTimeIdColumnAttendanceHistory : Migration
    {
        public override void Up()
        {
            Alter.Table("AttendanceHistories")
                .AddColumn("WorkTimeId").AsGuid().NotNullable();
        }
        public override void Down()
        {
            Delete.Column("WorkTimeId").FromTable("AttendanceHistories");
        }    
    }
}
