// rev 2026-09-19

namespace Ans.Net10.Common
{

	/// <summary>
	/// Абстрактный базовый класс для формирования постраничных моделей данных
	/// (DTO/Read-моделей) с автоматическим маппингом.
	/// </summary>
	/// <typeparam name="TEntity">Тип исходной доменной сущности СУБД.</typeparam>
	/// <typeparam name="TModel">Тип результирующей UI-модели или DTO.</typeparam>
	public abstract class _ListPaginatedModel_Base<TEntity, TModel>
		where TEntity : class
		where TModel : class
	{

		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="_ListPaginatedModel_Base{TEntity, TModel}"/>, 
		/// выполняя пагинацию запроса и безопасную однократную материализацию элементов.
		/// </summary>
		/// <param name="query">Исходный запрос <see cref="IQueryable{TEntity}"/> до применения ограничений.</param>
		/// <param name="func">Функция маппинга (проекции) из доменной сущности в выходную модель.</param>
		/// <param name="page">Номер запрашиваемой страницы (индексация с 1).</param>
		/// <param name="itemsOnPage">Количество отображаемых элементов на странице.</param>
		protected _ListPaginatedModel_Base(
			IQueryable<TEntity> query,
			Func<TEntity, TModel> func,
			int page,
			int itemsOnPage)
		{
			var helper1 = new PaginatedQueryableHelper<TEntity>(query, page, itemsOnPage);
			Pagination = new PaginationModel(helper1.PaginationHelper);
			Items = [.. helper1.Query.AsEnumerable().Select(func)];
			ItemsCount = Items.Count;
			HasItems = ItemsCount > 0;
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает модель состояния пагинации для интерфейсного слоя.
		/// </summary>
		public PaginationModel Pagination { get; }


		/// <summary>
		/// Возвращает материализованную коллекцию спроецированных выходных моделей текущей страницы.
		/// </summary>
		public IReadOnlyCollection<TModel> Items { get; }


		/// <summary>
		/// Возвращает фактическое количество элементов на текущей странице.
		/// </summary>
		public int ItemsCount { get; }


		/// <summary>
		/// Возвращает признак наличия элементов на текущей странице.
		/// </summary>
		public bool HasItems { get; }

	}

}
