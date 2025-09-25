using Microsoft.Extensions.DependencyInjection;
using RAttendanceSystem.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            var handlers = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Handler"))
                .ToList();

            foreach (var handler in handlers)
            {
                services.AddScoped(handler);
            }
        }
    }
}
