
using JSONCompareApi.Models;
using System;
using System.Runtime.Caching;

namespace JSONCompareApi.Service
{
    public class CacheService
    {
        ObjectCache _memoryCache = MemoryCache.Default;
        public JsonBase64Item GetData(string key)
        {
            try
            {
                JsonBase64Item item = (JsonBase64Item)_memoryCache.Get(key);
                return item;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public ObjectCache GetAllData()
        {
            try
            {
                return _memoryCache;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public bool SetData(string key, JsonBase64Item value, DateTimeOffset expirationTime)
        {
            bool res = true;
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    _memoryCache.Set(key, value, expirationTime);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return res;
        }
        public object RemoveData(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    return _memoryCache.Remove(key);
                }
            }
            catch (Exception e)
            {
                throw;
            }
            return false;
        }
    }
}
