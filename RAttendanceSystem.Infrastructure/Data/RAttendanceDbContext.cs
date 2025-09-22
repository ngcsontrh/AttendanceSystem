using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Data
{
    public class RAttendanceDbContext : DbContext
    {
        public RAttendanceDbContext(DbContextOptions<RAttendanceDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("app");
            modelBuilder.Entity<Employee>().ToTable("employee");
            modelBuilder.Entity<SystemNotification>().ToTable("system_notification");
            modelBuilder.Entity<Department>().ToTable("department");
            modelBuilder.Entity<Title>().ToTable("title");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<SystemNotification> SystemNotifications { get; set; }
    }
}
