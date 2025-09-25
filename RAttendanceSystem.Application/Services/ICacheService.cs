using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Application.Services
{
    public interface ICacheService
    {
        Task<bool> SetStringAsync(string key, string value, TimeSpan? expiration = null);
        Task<string?> GetStringAsync(string key);
        Task<bool> RemoveAsync(string key);
    }
}