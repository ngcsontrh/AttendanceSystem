using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations;
[Migration(202510281215)]
public class _202510281215_AddDepartmentTable : Migration
{
    public override void Up()
    {
        Create.Table("Departments")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Code").AsString(450).NotNullable()
            .WithColumn("Name").AsString(450).NotNullable()
            .WithColumn("Description").AsString(450).Nullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("CreatedById").AsGuid().Nullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable()
            .WithColumn("UpdatedById").AsGuid().Nullable();

        Insert.IntoTable("Departments").Row(new
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111110"),
            Code = "DPT001",
            Name = "Mặc định",
            Description = "Phòng ban mặc định",
            CreatedAt = DateTime.Now,
            CreatedById = (Guid?)null,
            UpdatedAt = (DateTime?)null,
            UpdatedById = (Guid?)null
        });
    }

    public override void Down()
    {
        Delete.Table("Departments");
    }
}
