using Microsoft.Extensions.Caching.Memory;

namespace Ans.Net8.Common
{

	public static class SuppCache
	{

		public static readonly MemoryCacheEntryOptions DEFAULT_CACHE_OPTIONS
			= new()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
			};


		/* functions */


		public static MemoryCacheEntryOptions GetOptions(
			int slidingExpirationSeconds,
			int absoluteExpirationRelativeToNowSeconds)
		{
			if (slidingExpirationSeconds <= 0 && absoluteExpirationRelativeToNowSeconds <= 0)
				return DEFAULT_CACHE_OPTIONS;
			var options1 = new MemoryCacheEntryOptions();
			if (slidingExpirationSeconds > 0)
				options1.SlidingExpiration = TimeSpan.FromSeconds(
					slidingExpirationSeconds);
			else
				options1.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(
					absoluteExpirationRelativeToNowSeconds);
			return options1;
		}

	}

}
