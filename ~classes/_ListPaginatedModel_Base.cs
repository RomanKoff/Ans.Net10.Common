namespace Ans.Net10.Common
{

	public class _ListPaginatedModel_Base<TEntity, TModel>
		where TEntity : class
		where TModel : class
	{

		/* ctor */


		public _ListPaginatedModel_Base(
			IQueryable<TEntity> query,
			Func<TEntity, TModel> func,
			int page,
			int itemsOnPage)
		{
			var helper1 = new PaginatedQueryableHelper<TEntity>(
				query, page, itemsOnPage);
			Pagination = new PaginationModel(helper1.PaginationHelper);
			Items = helper1.Query.Select(func).AsEnumerable();
			ItemsCount = Items.Count();
			HasItems = ItemsCount > 0;
		}


		/* readonly properties */


		public PaginationModel Pagination { get; }
		public IEnumerable<TModel> Items { get; }
		public int ItemsCount { get; }
		public bool HasItems { get; }

	}

}
