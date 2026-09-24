// rev 2026-09-21

using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Предоставляет вспомогательные методы для динамического построения LINQ-выражений и подготовки запросов.
	/// </summary>
	public static class SuppLinq
	{

		/// <summary>
		/// Безопасно объединяет существующий фильтр с новым выражением по логическому оператору "И" (AndAlso).
		/// Если исходный фильтр не задан, возвращает новое выражение.
		/// </summary>
		/// <typeparam name="T">Тип фильтруемой доменной сущности.</typeparam>
		/// <param name="filter">Текущее составное выражение-фильтр (допускает <see langword="null"/>).</param>
		/// <param name="expression">Новое добавляемое предикатное выражение.</param>
		/// <returns>Новое объединенное дерево выражений.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Expression<Func<T, bool>> ApplyFilter_Add<T>(
			Expression<Func<T, bool>>? filter,
			Expression<Func<T, bool>> expression)
		{
			return filter == null
				? expression : filter.And(expression);
		}


		/// <summary>
		/// Готовит и модифицирует LINQ-запрос: последовательно объединяет массив фильтров, 
		/// применяет их, рассчитывает метаданные пагинации на основе общего количества записей, 
		/// а также накладывает правила сортировки и постраничного секционирования.
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Исходный запрос <see cref="IQueryable{T}"/>.</param>
		/// <param name="filters">Массив предикатных выражений для фильтрации данных.</param>
		/// <param name="order">Строка определения полей и направлений сортировки (например, "Name,-Id").</param>
		/// <param name="page">Номер запрашиваемой страницы (индексация с 1).</param>
		/// <param name="itemsOnPage">Запрашиваемое количество элементов на одной странице.</param>
		/// <returns>Модифицированный запрос с примененными фильтрами, сортировкой и ограничениями Skip/Take.</returns>
		public static IQueryable<T> PrepareQuery<T>(
			IQueryable<T>? query,
			Expression<Func<T, bool>>[]? filters,
			string order,
			int page,
			int itemsOnPage)
		{
			if (query == null)
				return Enumerable.Empty<T>().AsQueryable();
			Expression<Func<T, bool>>? compositeFilter = null;
			if (filters != null && filters.Length > 0)
			{
				compositeFilter = filters[0];
				for (int i1 = 1; i1 < filters.Length; i1++)
					if (filters[i1] != null)
						compositeFilter = compositeFilter.And(filters[i1]);
			}
			if (compositeFilter != null)
				query = query.Where(compositeFilter);
			var pagination1 = new PaginatedDataModel(
				order, page, itemsOnPage, query.Count(), 25, 500);
			return pagination1.GetQueryTransform(query);
		}

	}

}
