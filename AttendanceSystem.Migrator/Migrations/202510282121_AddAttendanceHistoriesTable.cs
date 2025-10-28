using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations;
[Migration(202510282121)]
public class _202510282121_AddAttendanceHistoriesTable : Migration
{
    public override void Up()
    {
        Create.Table("AttendanceHistories")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("EmployeeId").AsGuid().NotNullable()
            .WithColumn("AttendanceDate").AsDateTime().NotNullable()
            .WithColumn("Type").AsInt32().NotNullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("CreatedById").AsGuid().Nullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("UpdatedById").AsGuid().Nullable();
    }

    public override void Down()
    {
        Delete.Table("AttendanceHistories");
    }
}
