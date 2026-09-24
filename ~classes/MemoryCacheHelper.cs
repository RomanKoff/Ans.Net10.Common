// rev 2026-09-23

using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для упрощения работы с кэшем в памяти <see cref="IMemoryCache"/>.
	/// Предоставляет потокобезопасные методы извлечения данных с автоматическим заполнением при их отсутствии.
	/// </summary>
	public class MemoryCacheHelper
	{

		private readonly IMemoryCache _cache;


		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="MemoryCacheHelper"/> с внедрением зависимости кэша
		/// и возможностью переопределения параметров кэширования по умолчанию.
		/// </summary>
		/// <param name="cache">Экземпляр службы кэширования в памяти.</param>
		/// <param name="defaultCacheOptions">
		/// Опциональные параметры кэширования по умолчанию.
		/// Если null — применяется <see cref="SuppCache.DEFAULT_CACHE_OPTIONS"/>.
		/// </param>
		public MemoryCacheHelper(
			IMemoryCache cache,
			MemoryCacheEntryOptions? defaultCacheOptions = null)
		{
			_cache = cache;
			DefaultCacheOptions = defaultCacheOptions ?? SuppCache.DEFAULT_CACHE_OPTIONS;
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает параметры времени жизни и ограничений записей кэша, применяемые по умолчанию.
		/// </summary>
		public MemoryCacheEntryOptions DefaultCacheOptions { get; }


		/* functions */


		/// <summary>
		/// Возвращает значение из кэша по указанному ключу. Если объект отсутствует, 
		/// выполняет его получение через переданный делегат-фабрику, сохраняет в кэш и возвращает результат.
		/// </summary>
		/// <typeparam name="T">Тип кэшируемого объекта.</typeparam>
		/// <param name="cacheKey">Уникальный строковый ключ записи кэша.</param>
		/// <param name="getObject">Делегат-фабрика для получения объекта, если он не найден в кэше.</param>
		/// <param name="options">
		/// Опциональные параметры времени жизни и ограничений записи кэша для текущего вызова.
		/// Если null — применяются <see cref="DefaultCacheOptions"/>.
		/// </param>
		/// <returns>Значение из кэша или вновь созданный объект типа <typeparamref name="T"/>.</returns>
		public T? Get<T>(
			string cacheKey,
			Func<T> getObject,
			MemoryCacheEntryOptions? options = null)
		{
			return _cache.GetOrCreate(
				cacheKey, x =>
				{
					x.SetOptions(options ?? DefaultCacheOptions);
					return getObject();
				});
		}


		/* methods */


		/// <summary>
		/// Принудительно удаляет запись из кэша по её уникальному ключу.
		/// </summary>
		/// <param name="cacheKey">Уникальный строковый ключ удаляемой записи.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Remove(
			string cacheKey)
		{
			_cache.Remove(cacheKey);
		}

	}

}
