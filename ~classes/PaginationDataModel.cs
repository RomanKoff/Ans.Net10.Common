namespace Ans.Net10.Common
{

	public class PaginationDataModel
	{

		/* ctor */


		public PaginationDataModel(
			string order,
			int page,
			int itemsOnPage,
			int totalItems,
			int defaultItemsOnPage,
			int maxItemsOnPages)
		{
			Order = string.IsNullOrEmpty(order)
				? null
				: new OrderBuilder(order);
			Page = page < 1 ? 1 : page;
			TotalItems = totalItems;
			ItemsOnPage = itemsOnPage == 0
				? defaultItemsOnPage
				: itemsOnPage > 0
					? itemsOnPage
					: maxItemsOnPages < 1
						? totalItems
						: maxItemsOnPages;
		}


		/* readonly properties */


		public OrderBuilder Order { get; }
		public int Page { get; }
		public int ItemsOnPage { get; }
		public int TotalItems { get; }

		public bool HasOrder
			=> Order != null;

		public bool HasPage
			=> ItemsOnPage > 0 && Page > 0;


		/* functions */


		public IQueryable<T> GetQueryTransform<T>(
			IQueryable<T> query)
		{
			var ordered1 = GetQueryOrdered(query);
			var pagining1 = GetQueryPaging(ordered1);
			return pagining1;
		}


		public IQueryable<T> GetQueryOrdered<T>(
			IQueryable<T> query)
		{
			return HasOrder
				? query.ApplyOrder(Order)
				: query;
		}


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
