using Microsoft.Extensions.Caching.Memory;

namespace Ans.Net10.Common
{

	public class WebGridCachedHelper<T>
	{

		private readonly WebGridHelper<T> _api;
		private readonly IMemoryCache _cache;


		/* ctor */


		public WebGridCachedHelper(
			HttpClient httpClient,
			IMemoryCache cache,
			Func<string, T> parser,
			string url,
			string cacheKey,
			MemoryCacheEntryOptions cacheOptions)
		{
			_api = new(httpClient, url, parser);
			_cache = cache;
			CacheKey = cacheKey;
			CacheOptions = cacheOptions;
		}


		/* readonly properties */


		public string CacheKey { get; }


		public MemoryCacheEntryOptions CacheOptions { get; }


		/* functions */


		public WebGridResult<T> SendQuery()
		{
			if (!_cache.TryGetValue(CacheKey, out WebGridResult<T> value1))
			{
				value1 = _api.SendQuery();
				_cache.Set(
					CacheKey, value1, CacheOptions ?? _Consts.DEFAULT_CACHE_OPTIONS);
			}
			return value1;
		}


		/* methods */


		public void CacheClear()
		{
			_cache.Remove(CacheKey);
		}

	}

}
