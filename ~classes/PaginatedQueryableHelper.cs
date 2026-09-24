// rev 2026-09-19

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для одновременного расчета метаданных пагинации
	/// и формирования постраничного LINQ-запроса.
	/// </summary>
	/// <typeparam name="TEntity">Тип сущности в запросе, принадлежащий к ссылочным типам.</typeparam>
	public class PaginatedQueryableHelper<TEntity>
		where TEntity : class
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PaginatedQueryableHelper{TEntity}"/>, 
		/// выполняя расчет общего количества элементов и подготавливая постраничный запрос.
		/// </summary>
		/// <param name="query">
		/// Исходный запрос <see cref="IQueryable{TEntity}"/> до применения пагинации.
		/// </param>
		/// <param name="page">Номер запрашиваемой страницы (индексация с 1).</param>
		/// <param name="itemsOnPage">
		/// Количество отображаемых элементов на одной странице. Должно быть больше 0.
		/// </param>
		public PaginatedQueryableHelper(
			IQueryable<TEntity> query,
			int page,
			int itemsOnPage)
		{
			page = page < 1
				? 1 : page;
			itemsOnPage = itemsOnPage < 1
				? 1 : itemsOnPage;
			var totalItems1 = query.Count();
			Query = query
				.Skip((page - 1) * itemsOnPage)
				.Take(itemsOnPage);
			PaginationHelper = new PaginationHelper(
				itemsOnPage,
				totalItems1,
				page);
		}


		/* reaonly properties */


		/// <summary>
		/// Возвращает вычисленный хелпер навигации пагинации
		/// с актуальными состояниями и диапазонами страниц.
		/// </summary>
		public PaginationHelper PaginationHelper { get; }


		/// <summary>
		/// Возвращает модифицированный запрос <see cref="IQueryable{TEntity}"/>
		/// с примененными операторами Skip и Take.
		/// </summary>
		public IQueryable<TEntity> Query { get; }

	}

}
