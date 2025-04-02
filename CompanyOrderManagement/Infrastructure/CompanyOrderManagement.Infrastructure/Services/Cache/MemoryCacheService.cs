using CompanyOrderManagement.Application.Logging.Services.Interfaces;
using CompanyOrderManagement.Application.Services.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyOrderManagement.Infrastructure.Services.Cache
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILoggerService _logger;

        public MemoryCacheService(IMemoryCache memoryCache, ILoggerService logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public T Get<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T value))
            {
                _logger.Info($"Cache hit for key: {key}");
                return value;
            }

            _logger.Warn($"Cache miss for key: {key}");
            return default;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            _logger.Info($"Cache entry removed for key: {key}");

        }

        public void Set<T>(string key, T value, TimeSpan absoluteExpiration, TimeSpan? slidingExpiration = null)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration,
                SlidingExpiration = slidingExpiration
            };

            _memoryCache.Set(key, value, cacheEntryOptions);
            _logger.Info($"Cache entry added or updated for key: {key} with AbsoluteExpiration: {absoluteExpiration}, SlidingExpiration: {slidingExpiration}");
        }
    }
}
