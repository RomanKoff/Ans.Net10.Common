namespace Ans.Net10.Common
{

	public static partial class _e_PaginationDataModel
	{

		/* functions */


		public static IQueryable<T> GetQueryTransform<T>(
			this PaginationDataModel data,
			IQueryable<T> query)
		{
			var ordered1 = data.GetQueryOrdered(query);
			var pagining1 = data.GetQueryPaging(ordered1);
			return pagining1;
		}


		public static IQueryable<T> GetQueryOrdered<T>(
			this PaginationDataModel data,
			IQueryable<T> query)
		{
			return data.HasOrder
				? query.ApplyOrder(data.Order)
				: query;
		}


		public static IQueryable<T> GetQueryPaging<T>(
			this PaginationDataModel data,
			IQueryable<T> query)
		{
			return data.HasPage
				? query
					.Skip((data.Page - 1) * data.ItemsOnPage)
					.Take(data.ItemsOnPage)
				: query;
		}

	}

}
