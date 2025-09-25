using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RAttendanceSystem.Application.Services;
using StackExchange.Redis;

namespace RAttendanceSystem.Infrastructure.Services
{
    internal class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(IConnectionMultiplexer redis, ILogger<RedisCacheService> logger)
        {
            _database = redis.GetDatabase();
            _logger = logger;
        }


        public async Task<string?> GetStringAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiration = null)
        {
            return await _database.StringSetAsync(key, value, expiration);
        }
    }
}