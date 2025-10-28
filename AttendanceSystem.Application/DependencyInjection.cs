using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {        
        services.AddHandlers();
        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        var commandHandlers = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("CommandHandler"))
            .ToList();
        var queryHandlers = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("QueryHandler"))
            .ToList();
        foreach (var handler in commandHandlers.Concat(queryHandlers))
        {
            services.AddScoped(handler);
        }
        return services;
    }
}
