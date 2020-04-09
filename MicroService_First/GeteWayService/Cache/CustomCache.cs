using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeteWayService.Cache
{
    /// <summary>
    /// 自定义缓存--自行扩展  redis
    /// </summary>
    public class CustomCache : IOcelotCache<CachedResponse>
    {
        private class CacheDataModel
        {
            public CachedResponse CacheResponse { get; set; }

            public DateTime Timeout { get; set; }

            public string Region { get; set; }
        }

        private static Dictionary<string, CacheDataModel> keys = new Dictionary<string, CacheDataModel>();

        public void Add(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            keys[key] = new CacheDataModel()
            {
                CacheResponse = value,
                Region = region,
                Timeout = DateTime.Now.Add(ttl)
            };
        }

        public CachedResponse Get(string key, string region)
        {
            if (keys.ContainsKey(key) 
                && keys[key] != null
                && keys[key].Timeout > DateTime.Now
                && keys[key].Region.Equals(region))
            {
                return keys[key].CacheResponse;
            }
            else
            {
                return null;
            }
        }

        public void ClearRegion(string region)
        {
            var keyList = keys.Where(x => x.Value.Region.Equals(region)).Select(x => x.Key);
            foreach (var item in keyList)
            {
                keys.Remove(item);
            }
        }

        public void AddAndDelete(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            keys[key] = new CacheDataModel()
            {
                CacheResponse = value,
                Region = region,
                Timeout = DateTime.Now.Add(ttl)
            };
        }
    }
}
