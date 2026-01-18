using System.Linq.Expressions;

namespace Ans.Net10.Common
{

	public static class SuppLinq
	{

		/* functions */


		public static Expression<Func<T, bool>> ApplyFilter_Add<T>(
			Expression<Func<T, bool>> filter,
			Expression<Func<T, bool>> expression)
		{
			return filter == null
				? expression : filter.And(expression);
		}


		public static IQueryable<T> PrepareQuery<T>(
			IQueryable<T> query,
			Expression<Func<T, bool>>[] filters,
			string order,
			int page,
			int itemsOnPage)
		{
			Expression<Func<T, bool>> filter1 = null;
			if (filters?.Length > 0)
			{
				filter1 = filters[0];
				foreach (var item1 in filters.Skip(1))
					filter1 = filter1.And(item1);
			}
			if (filter1 != null)
				query = query.Where(filter1);

			var pagination1 = new PaginationDataModel(
				order, page, itemsOnPage, query.Count(), 25, 500);

			query = pagination1.GetQueryTransform(query);
			return query;
		}

	}

}
