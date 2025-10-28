using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations;

[Migration(202510282128)]
public class _202510282128_AddWorkTimeTable : Migration
{
    public override void Up()
    {
        Create.Table("WorkTimes")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(450).NotNullable()    
            .WithColumn("ValidCheckInTime").AsTime().NotNullable()
            .WithColumn("ValidCheckOutTime").AsTime().NotNullable()
            .WithColumn("IsActive").AsBoolean().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("CreatedById").AsGuid().Nullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("UpdatedById").AsGuid().Nullable();
    }

    public override void Down()
    {
        Delete.Table("WorkTimes");
    }
}
