using FluentMigrator;
using FluentMigrator.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Migrator.Migrations;

[Migration(202510282100)]
public class _202510282100_AddIdentityTables : Migration
{
    public override void Up()
    {
        Create.Table("AspNetRoles")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(256).Nullable()
            .WithColumn("NormalizedName").AsString(256).Nullable()
            .WithColumn("ConcurrencyStamp").AsString(450).Nullable();

        Create.Table("AspNetUsers")
            .WithColumn("Id").AsGuid().NotNullable().PrimaryKey()            
            .WithColumn("UserName").AsString(256).Nullable()
            .WithColumn("NormalizedUserName").AsString(256).Nullable()
            .WithColumn("Email").AsString(256).Nullable()
            .WithColumn("NormalizedEmail").AsString(256).Nullable()
            .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
            .WithColumn("PasswordHash").AsString(450).Nullable()
            .WithColumn("SecurityStamp").AsString(450).Nullable()
            .WithColumn("ConcurrencyStamp").AsString(450).Nullable()
            .WithColumn("PhoneNumber").AsString(450).Nullable()
            .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
            .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
            .WithColumn("LockoutEnd").AsDateTimeOffset().Nullable()
            .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
            .WithColumn("AccessFailedCount").AsInt32().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("UpdatedAt").AsDateTime().Nullable();

        Create.Table("AspNetRoleClaims")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("RoleId").AsGuid().NotNullable()
            .WithColumn("ClaimType").AsString(450).Nullable()
            .WithColumn("ClaimValue").AsString(450).Nullable();

        Create.ForeignKey("FK_AspNetRoleClaims_AspNetRoles_RoleId")
            .FromTable("AspNetRoleClaims").ForeignColumn("RoleId")
            .ToTable("AspNetRoles").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table("AspNetUserClaims")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("UserId").AsGuid().NotNullable()
            .WithColumn("ClaimType").AsString(450).Nullable()
            .WithColumn("ClaimValue").AsString(450).Nullable();

        Create.ForeignKey("FK_AspNetUserClaims_AspNetUsers_UserId")
            .FromTable("AspNetUserClaims").ForeignColumn("UserId")
            .ToTable("AspNetUsers").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table("AspNetUserLogins")
            .WithColumn("LoginProvider").AsString(128).NotNullable().PrimaryKey()
            .WithColumn("ProviderKey").AsString(128).NotNullable().PrimaryKey()
            .WithColumn("ProviderDisplayName").AsString(450).Nullable()
            .WithColumn("UserId").AsGuid().NotNullable();

        Create.ForeignKey("FK_AspNetUserLogins_AspNetUsers_UserId")
            .FromTable("AspNetUserLogins").ForeignColumn("UserId")
            .ToTable("AspNetUsers").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table("AspNetUserRoles")
            .WithColumn("UserId").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("RoleId").AsGuid().NotNullable().PrimaryKey();

        Create.ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId")
            .FromTable("AspNetUserRoles").ForeignColumn("UserId")
            .ToTable("AspNetUsers").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId")
            .FromTable("AspNetUserRoles").ForeignColumn("RoleId")
            .ToTable("AspNetRoles").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Table("AspNetUserTokens")
            .WithColumn("UserId").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("LoginProvider").AsString(128).NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(128).NotNullable().PrimaryKey()
            .WithColumn("Value").AsString(450).Nullable();

        Create.ForeignKey("FK_AspNetUserTokens_AspNetUsers_UserId")
            .FromTable("AspNetUserTokens").ForeignColumn("UserId")
            .ToTable("AspNetUsers").PrimaryColumn("Id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);

        Create.Index("IX_AspNetRoleClaims_RoleId")
            .OnTable("AspNetRoleClaims")
            .OnColumn("RoleId").Ascending()
            .WithOptions().NonClustered();

        Create.Index("RoleNameIndex")
            .OnTable("AspNetRoles")
            .OnColumn("NormalizedName").Ascending()
            .WithOptions().Unique()
            .WithOptions().Filter("([NormalizedName] IS NOT NULL)");

        Create.Index("IX_AspNetUserClaims_UserId")
            .OnTable("AspNetUserClaims")
            .OnColumn("UserId").Ascending()
            .WithOptions().NonClustered();

        Create.Index("IX_AspNetUserLogins_UserId")
            .OnTable("AspNetUserLogins")
            .OnColumn("UserId").Ascending()
            .WithOptions().NonClustered();

        Create.Index("IX_AspNetUserRoles_RoleId")
            .OnTable("AspNetUserRoles")
            .OnColumn("RoleId").Ascending()
            .WithOptions().NonClustered();

        Create.Index("EmailIndex")
            .OnTable("AspNetUsers")
            .OnColumn("NormalizedEmail").Ascending()
            .WithOptions().NonClustered();

        Create.Index("UserNameIndex")
            .OnTable("AspNetUsers")
            .OnColumn("NormalizedUserName").Ascending()
            .WithOptions().Unique()
            .WithOptions().Filter("([NormalizedUserName] IS NOT NULL)");

        Insert.IntoTable("AspNetUsers").Row(new
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111000"),
            AccessFailedCount = 0,
            ConcurrencyStamp = "b0d6a8b0-9c0a-4b1a-9f8e-8c8e1a5a6d0c",            
            CreatedAt = new DateTime(2025, 10, 28, 0, 0, 0, 0),
            Email = "admin@AttendanceSystem.com",
            NormalizedEmail = "ADMIN@AttendanceSystem.COM",
            EmailConfirmed = false,
            LockoutEnabled = false,            
            LockoutEnd = (DateTimeOffset?)null,
            UserName = "admin@AttendanceSystem.com",
            NormalizedUserName = "ADMIN@AttendanceSystem.COM",
            PasswordHash = "AQAAAAIAAYagAAAAEEtIB125J+TkTo2ePSKkL2vxR2G7JroUFNxuL0IB2L6loR2oSIFfy3kdp4XYYSsS3g==",
            PhoneNumber = (string?)null,
            PhoneNumberConfirmed = false,
            SecurityStamp = "c5f0fafa-3f6c-4f8a-9e2a-4a8f8d7b3d1f",
            TwoFactorEnabled = false,
            UpdatedAt = (DateTime?)null,
        });

        Insert.IntoTable("AspNetRoles").Row(new
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111110000"),
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "d1f5e6c7-8b9a-4c3d-9e0f-1a2b3c4d5e6f"
        });

        Insert.IntoTable("AspNetUserRoles").Row(new
        {
            UserId = Guid.Parse("11111111-1111-1111-1111-111111111000"),
            RoleId = Guid.Parse("11111111-1111-1111-1111-111111110000")
        });
    }

    public override void Down()
    {
        Delete.Table("AspNetUserTokens");
        Delete.Table("AspNetUserRoles");
        Delete.Table("AspNetUserLogins");
        Delete.Table("AspNetUserClaims");
        Delete.Table("AspNetUsers");
        Delete.Table("AspNetRoleClaims");
        Delete.Table("AspNetRoles");
    }
}
