using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations;

[Migration(202510282127)]
public class _202510282127_AddLeaveRequestTable : Migration
{
    public override void Up()
    {
        Create.Table("LeaveRequests")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("EmployeeId").AsGuid().NotNullable()
            .WithColumn("StartDate").AsDateTime().NotNullable()
            .WithColumn("EndDate").AsDateTime().NotNullable()
            .WithColumn("Reason").AsString(450).NotNullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("ApprovedById").AsGuid().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("CreatedById").AsGuid().Nullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("UpdatedById").AsGuid().Nullable();
    }
    public override void Down()
    {
        Delete.Table("LeaveRequests");
    }
}
