using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations;
[Migration(202510282125)]
public class _202510282125_AddEmployeeTable : Migration
{
    public override void Up()
    {
        Create.Table("Employees")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Code").AsString(450).NotNullable()
            .WithColumn("FullName").AsString(450).NotNullable()
            .WithColumn("Email").AsString(450).NotNullable()
            .WithColumn("Gender").AsInt32().NotNullable()
            .WithColumn("DepartmentId").AsGuid().NotNullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("ManagerId").AsGuid().Nullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("CreatedById").AsGuid().Nullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("UpdatedById").AsGuid().Nullable()
            .WithColumn("UserId").AsGuid().Nullable();

        Insert.IntoTable("Employees").Row(new
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111100"),
            Code = "EMP001",
            FullName = "Admin User",
            Email = "admin@AttendanceSystem.com",
            Gender = 1,
            DepartmentId = Guid.Parse("11111111-1111-1111-1111-111111111110"),
            Status = 0,
            ManagerId = (Guid?)null,
            CreatedAt = DateTime.Now,
            CreatedById = (Guid?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedById = (Guid?)null,
            UserId = Guid.Parse("11111111-1111-1111-1111-111111111000"),
        });
    }

    public override void Down()
    {
        Delete.Table("Employees");
    }
}
