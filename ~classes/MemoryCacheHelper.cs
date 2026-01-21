using Microsoft.Extensions.Caching.Memory;

namespace Ans.Net10.Common
{

	public class MemoryCacheHelper
	{

		private readonly IMemoryCache _cache;


		/* ctor */


		public MemoryCacheHelper(
			IMemoryCache cache)
		{
			_cache = cache;
		}


		/* functions */


		public T Get<T>(
			string cacheKey,
			Func<T> getObject,
			MemoryCacheEntryOptions options = null)
		{
			if (!_cache.TryGetValue(cacheKey, out T value1))
			{
				value1 = getObject();
				_cache.Set(cacheKey, value1, options ?? _Consts.DEFAULT_CACHE_OPTIONS);
			}
			return value1;
		}


		/* methods */


		public void Remove(
			string cacheKey)
		{
			_cache.Remove(cacheKey);
		}

	}

}
