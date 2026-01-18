using System.Linq.Expressions;

namespace Ans.Net10.Common
{

	public static partial class __e_linq
	{

		/* IQueryable order  */


		public static IOrderedQueryable<T> ApplyOrder<T>(
			this IQueryable<T> query,
			OrderBuilder order)
		{
			if (order.Items.Length > 0)
			{
				var query1 = (order.Items[0].IsDescending)
					? query.ApplyOrderByDescending(order.Items[0].Column)
					: query.ApplyOrderBy(order.Items[0].Column);
				foreach (var item1 in order.Items.Skip(1))
					query1 = (item1.IsDescending)
						? query1.ApplyThenByDescending(item1.Column)
						: query1.ApplyThenBy(item1.Column);
				return query1;
			}
			return (IOrderedQueryable<T>)query;
		}


		public static IOrderedQueryable<T> ApplyOrderBy<T>(
			this IQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "OrderBy");
		}


		public static IOrderedQueryable<T> ApplyOrderByDescending<T>(
			this IQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "OrderByDescending");
		}


		public static IOrderedQueryable<T> ApplyThenBy<T>(
			this IOrderedQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "ThenBy");
		}


		public static IOrderedQueryable<T> ApplyThenByDescending<T>(
			this IOrderedQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "ThenByDescending");
		}


		/* Expressions */


		public static Expression<Func<T, bool>> Or<T>(
			this Expression<Func<T, bool>> expr1,
			Expression<Func<T, bool>> expr2)
		{
			var e1 = Expression.Invoke(
				expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(
				Expression.OrElse(expr1.Body, e1), expr1.Parameters);
		}


		public static Expression<Func<T, bool>> And<T>(
			this Expression<Func<T, bool>> expr1,
			Expression<Func<T, bool>> expr2)
		{
			var e1 = Expression.Invoke(
				expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(
				Expression.AndAlso(expr1.Body, e1), expr1.Parameters);
		}


		/* privates */


		private static IOrderedQueryable<T> _applyOrder<T>(
			IQueryable<T> query,
			string property,
			string methodName)
		{
			var type1 = typeof(T);
			var props1 = property.Split('.');
			var arg1 = Expression.Parameter(type1, "x");
			Expression expr1 = arg1;
			foreach (var prop1 in props1)
			{
				var info1 = type1.GetProperty(prop1);
				expr1 = Expression.Property(expr1, info1);
				type1 = info1.PropertyType;
			}
			var type2 = typeof(Func<,>).MakeGenericType(typeof(T), type1);
			var expr2 = Expression.Lambda(type2, expr1, arg1);
			var result1 = typeof(Queryable).GetMethods().Single(
				x => x.Name == methodName
					&& x.IsGenericMethodDefinition
					&& x.GetGenericArguments().Length == 2
					&& x.GetParameters().Length == 2)
						.MakeGenericMethod(typeof(T), type1)
						.Invoke(null, [query, expr2]);
			return (IOrderedQueryable<T>)result1;
		}

	}

}
