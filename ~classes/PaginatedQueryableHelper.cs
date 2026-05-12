namespace Ans.Net10.Common
{

	public class PaginatedQueryableHelper<TEntity>
		where TEntity : class
	{
		public PaginatedQueryableHelper(
			IQueryable<TEntity> query,
			int page,
			int itemsOnPage)
		{
			var totalItems1 = query.Count();
			Query = query
				.Skip((page - 1) * itemsOnPage)
				.Take(itemsOnPage);
			PaginationHelper = new(itemsOnPage, totalItems1, page);
		}

		public PaginationHelper PaginationHelper { get; }
		public IQueryable<TEntity> Query { get; }
	}

}
