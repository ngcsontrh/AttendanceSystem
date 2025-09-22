using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddDbContext<RAttendanceDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            
            var repositories = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
                .ToList();
            foreach (var repo in repositories)
            {
                var interfaceType = repo.GetInterface("I" + repo.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repo);
                }
            }
        }
    }
}
