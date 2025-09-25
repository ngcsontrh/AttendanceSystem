using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RAttendanceSystem.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAttendanceSystem.Infrastructure.Services
{
    internal class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<MemoryCacheService> _logger;

        public MemoryCacheService(IMemoryCache memoryCache, ILogger<MemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public Task<string?> GetStringAsync(string key)
        {
            return Task.FromResult(_memoryCache.Get<string>(key));
        }

        public Task<bool> RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.FromResult(true);
        }

        public Task<bool> SetStringAsync(string key, string value, TimeSpan? expiration = null)
        {
            _memoryCache.Set(key, value, expiration ?? TimeSpan.FromHours(1));
            return Task.FromResult(true);
        }
    }
}
