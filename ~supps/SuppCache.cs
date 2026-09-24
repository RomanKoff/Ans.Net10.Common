// rev 2026-09-16

using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для работы с кэшем в памяти.
	/// </summary>
	public static class SuppCache
	{

		/* consts */


		/// <summary>
		/// Настройки кэширования по умолчанию (абсолютное время жизни — 10 секунд).
		/// </summary>
		public static readonly MemoryCacheEntryOptions DEFAULT_CACHE_OPTIONS = new()
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
		};


		/// <summary>
		/// Настройки кэширования с нулевым временем (отключенное кэширование).
		/// </summary>
		public static readonly MemoryCacheEntryOptions ZERO_CACHE_OPTIONS = new()
		{
			AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1)
		};


		/* functions */


		/// <summary>
		/// Формирует параметры конфигурации записи кэша на основе переданных интервалов в секундах.
		/// </summary>
		/// <param name="slidingExpirationSeconds">
		/// Скользящее время жизни записи в секундах (сбрасывается при каждом обращении).
		/// </param>
		/// <param name="absoluteExpirationRelativeToNowSeconds">
		/// Абсолютное время жизни записи в секундах относительно текущего момента.
		/// </param>
		/// <returns>Объект конфигурации <see cref="MemoryCacheEntryOptions"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
			if (absoluteExpirationRelativeToNowSeconds > 0)
				options1.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(
					absoluteExpirationRelativeToNowSeconds);
			return options1;
		}

	}

}
