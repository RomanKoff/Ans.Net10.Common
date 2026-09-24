// rev 2026-09-19

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Модель данных, объединяющая параметры сортировки
	/// и динамической пагинации для применения к запросам.
	/// </summary>
	public class PaginatedDataModel
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PaginatedDataModel"/>.
		/// </summary>
		/// <param name="order">Строка определения сортировки (например, "Name,-Age").</param>
		/// <param name="page">Номер запрашиваемой страницы (индексация с 1).</param>
		/// <param name="itemsOnPage">Запрашиваемое количество элементов на странице. Если меньше или равно 0, рассчитывается автоматически.</param>
		/// <param name="totalItems">Общее количество элементов в коллекции.</param>
		/// <param name="defaultItemsOnPage">Количество элементов на странице по умолчанию (используется, если запрашиваемое равно 0).</param>
		/// <param name="maxItemsOnPages">Максимально допустимое количество элементов на странице для защиты от перегрузки.</param>
		public PaginatedDataModel(
			string? order,
			int page,
			int itemsOnPage,
			int totalItems,
			int defaultItemsOnPage,
			int maxItemsOnPages)
		{
			Order = string.IsNullOrEmpty(order)
				? null : new OrderBuilder(order);
			Page = page < 1
				? 1 : page;
			TotalItems = totalItems;
			if (itemsOnPage == 0)
				itemsOnPage = defaultItemsOnPage;
			else if (itemsOnPage < 0)
				itemsOnPage = maxItemsOnPages < 1
					? totalItems : maxItemsOnPages;
			if (maxItemsOnPages > 0 && itemsOnPage > maxItemsOnPages)
				itemsOnPage = maxItemsOnPages;
			ItemsOnPage = itemsOnPage;
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает объект построителя сортировки. Может быть равен <see langword="null"/>,
		/// если сортировка не задана.
		/// </summary>
		public OrderBuilder? Order { get; }


		/// <summary>
		/// Возвращает номер текущей страницы. Гарантированно не меньше 1.
		/// </summary>
		public int Page { get; }


		/// <summary>
		/// Возвращает рассчитанное количество элементов на одной странице.
		/// </summary>
		public int ItemsOnPage { get; }


		/// <summary>
		/// Возвращает общее количество элементов в коллекции.
		/// </summary>
		public int TotalItems { get; }


		/// <summary>
		/// Возвращает признак наличия параметров сортировки.
		/// </summary>
		public bool HasOrder
			=> Order != null;


		/// <summary>
		/// Возвращает признак активности пагинации.
		/// </summary>
		public bool HasPage
			=> ItemsOnPage > 0 && Page > 0;


		/* functions */


		/// <summary>
		/// Применяет к запросу последовательно сортировку и параметры пагинации.
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Исходный запрос <see cref="IQueryable{T}"/>.</param>
		/// <returns>Запрос с примененной сортировкой и ограничениями пагинации.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IQueryable<T> GetQueryTransform<T>(
			IQueryable<T> query)
		{
			return GetQueryPaging(GetQueryOrdered(query));
		}


		/// <summary>
		/// Применяет к запросу параметры сортировки, если они заданы.
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Исходный запрос <see cref="IQueryable{T}"/>.</param>
		/// <returns>Запрос с примененной сортировкой или исходный запрос.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IQueryable<T> GetQueryOrdered<T>(
			IQueryable<T> query)
		{
			return HasOrder
				? query.ApplyOrder(Order!) : query;
		}


		/// <summary>
		/// Применяет к запросу операторы секционирования (Skip/Take)
		/// на основе текущей страницы и размера страницы.
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Исходный запрос <see cref="IQueryable{T}"/>.</param>
		/// <returns>Запрос с примененными операторами Skip и Take или исходный запрос.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IQueryable<T> GetQueryPaging<T>(
			IQueryable<T> query)
		{
			return HasPage
				? query
					.Skip((Page - 1) * ItemsOnPage)
					.Take(ItemsOnPage)
				: query;
		}

	}

}
