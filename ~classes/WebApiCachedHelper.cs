using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Ans.Net10.Common
{

	public class WebApiCachedHelper<T>
		: IWebApiHelper<T>
	{

		private readonly WebApiHelper<T> _api;
		private readonly IMemoryCache _cache;


		/* ctor */


		public WebApiCachedHelper(
			HttpClient httpClient,
			IMemoryCache cache,
			string baseUrl,
			string cacheKey,
			MemoryCacheEntryOptions cacheOptions,
			JsonSerializerOptions jsonOptions)
		{
			_api = new(httpClient, baseUrl, jsonOptions);
			_cache = cache;
			CacheKey = cacheKey;
			CacheOptions = cacheOptions;
		}


		/* readonly properties */


		public string CacheKey { get; }


		public string BaseUrl
			=> _api.BaseUrl;


		public JsonSerializerOptions JsonOptions
			=> _api.JsonOptions;


		public ParamsBuilder Params
			=> _api.Params;


		public MemoryCacheEntryOptions CacheOptions { get; }


		/* functions */


		public WebApiResult<T> SendQuery(
			string queryString)
		{
			if (!_cache.TryGetValue(CacheKey, out WebApiResult<T> value1))
			{
				value1 = _api.SendQuery(queryString);
				_cache.Set(
					CacheKey, value1, CacheOptions ?? _Consts.DEFAULT_CACHE_OPTIONS);
			}
			return value1;
		}


		public WebApiResult<T> SendQuery()
		{
			return SendQuery(Params.ToString());
		}


		/* methods */


		public void CacheClear()
		{
			_cache.Remove(CacheKey);
		}

	}

}
