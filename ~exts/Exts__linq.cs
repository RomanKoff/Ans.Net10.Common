// rev 2026-09-17

using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class Exts__linq
	{

		/* IQueryable order */


		/// <summary>
		/// Применяет цепочку сортировок к запросу на основе объекта конфигурации порядка сортировки.
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Исходный запрос.</param>
		/// <param name="order">Объект построителя порядка сортировки.</param>
		/// <returns>Запрос с примененной последовательностью сортировок.</returns>
		public static IOrderedQueryable<T> ApplyOrder<T>(
			this IQueryable<T> query,
			OrderBuilder order)
		{
			ArgumentNullException.ThrowIfNull(query);
			ArgumentNullException.ThrowIfNull(order);
			if (order.Items.Length > 0)
			{
				var query1 = (order.Items[0].IsDescending)
					? query.ApplyOrderByDescending(order.Items[0].Column)
					: query.ApplyOrderBy(order.Items[0].Column);
				for (int i1 = 1; i1 < order.Items.Length; i1++)
				{
					var item1 = order.Items[i1];
					query1 = (item1.IsDescending)
						? query1.ApplyThenByDescending(item1.Column)
						: query1.ApplyThenBy(item1.Column);
				}
				return query1;
			}
			return (IOrderedQueryable<T>)query;
		}


		/// <summary>
		/// Применяет первичную сортировку по возрастанию для указанного строкового
		/// имени свойства (поддерживает вложенность через точку).
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Исходный запрос.</param>
		/// <param name="property">Имя свойства для сортировки (например, "Name" или "Category.Title").</param>
		/// <returns>Запрос с примененной сортировкой.</returns>
		/// <exception cref="InvalidOperationException">
		/// Вызывается, если указанное свойство не найдено в типе сущности.
		/// </exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IOrderedQueryable<T> ApplyOrderBy<T>(
			this IQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "OrderBy");
		}


		/// <summary>
		/// Применяет первичную сортировку по убыванию для указанного строкового
		/// имени свойства (поддерживает вложенность через точку).
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Исходный запрос.</param>
		/// <param name="property">Имя свойства для сортировки.</param>
		/// <returns>Запрос с примененной сортировкой.</returns>
		/// <exception cref="InvalidOperationException">
		/// Вызывается, если указанное свойство не найдено в типе сущности.
		/// </exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IOrderedQueryable<T> ApplyOrderByDescending<T>(
			this IQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "OrderByDescending");
		}


		/// <summary>
		/// Применяет последующую сортировку по возрастанию для указанного строкового
		/// имени свойства (поддерживает вложенность через точку).
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Отсортированный запрос.</param>
		/// <param name="property">Имя свойства для последующей сортировки.</param>
		/// <returns>Запрос с примененной последующей сортировкой.</returns>
		/// <exception cref="InvalidOperationException">
		/// Вызывается, если указанное свойство не найдено в типе сущности.
		/// </exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IOrderedQueryable<T> ApplyThenBy<T>(
			this IOrderedQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "ThenBy");
		}


		/// <summary>
		/// Применяет последующую сортировку по убыванию для указанного строкового
		/// имени свойства (поддерживает вложенность через точку).
		/// </summary>
		/// <typeparam name="T">Тип сущности в запросе.</typeparam>
		/// <param name="query">Отсортированный запрос.</param>
		/// <param name="property">Имя свойства для последующей сортировки.</param>
		/// <returns>Запрос с примененной последующей сортировкой.</returns>
		/// <exception cref="InvalidOperationException">
		/// Вызывается, если указанное свойство не найдено в типе сущности.
		/// </exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IOrderedQueryable<T> ApplyThenByDescending<T>(
			this IOrderedQueryable<T> query,
			string property)
		{
			return _applyOrder(query, property, "ThenByDescending");
		}


		/* Expressions */


		/// <summary>
		/// Объединяет два выражения условий логическим оператором ИЛИ (OrElse).
		/// </summary>
		/// <typeparam name="T">Тип фильтруемого объекта.</typeparam>
		/// <param name="expr1">Первое выражение-предикат.</param>
		/// <param name="expr2">Второе выражение-предикат.</param>
		/// <returns>Новое комбинированное дерево выражений.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Expression<Func<T, bool>> Or<T>(
			this Expression<Func<T, bool>> expr1,
			Expression<Func<T, bool>> expr2)
		{
			ArgumentNullException.ThrowIfNull(expr1);
			ArgumentNullException.ThrowIfNull(expr2);
			var e1 = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, e1), expr1.Parameters);
		}


		/// <summary>
		/// Объединяет два выражения условий логическим оператором И (AndAlso).
		/// </summary>
		/// <typeparam name="T">Тип фильтруемого объекта.</typeparam>
		/// <param name="expr1">Первое выражение-предикат.</param>
		/// <param name="expr2">Второе выражение-предикат.</param>
		/// <returns>Новое комбинированное дерево выражений.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Expression<Func<T, bool>> And<T>(
			this Expression<Func<T, bool>> expr1,
			Expression<Func<T, bool>> expr2)
		{
			ArgumentNullException.ThrowIfNull(expr1);
			ArgumentNullException.ThrowIfNull(expr2);
			var e1 = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
			return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, e1), expr1.Parameters);
		}


		/* privates */


		private static readonly Dictionary<string, MethodInfo> _queryableMethods = typeof(Queryable)
			.GetMethods(BindingFlags.Public | BindingFlags.Static)
			.Where(m => m.IsGenericMethodDefinition
				&& m.GetGenericArguments().Length == 2
				&& m.GetParameters().Length == 2)
			.GroupBy(m => m.Name)
			.ToDictionary(g => g.Key, g => g.First());


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
				var info1 = type1.GetProperty(prop1)
					?? throw new InvalidOperationException($"[Ans.Net10.Common] The property '{prop1}' was not found in the type '{type1.Name}'.");
				expr1 = Expression.Property(expr1, info1);
				type1 = info1.PropertyType;
			}
			var type2 = typeof(Func<,>).MakeGenericType(typeof(T), type1);
			var expr2 = Expression.Lambda(type2, expr1, arg1);
			if (!_queryableMethods.TryGetValue(methodName, out var methodTemplate))
				throw new InvalidOperationException($"[Ans.Net10.Common] The '{methodName}' method is not supported by the Queryable subsystem.");
			var result1 = methodTemplate
				.MakeGenericMethod(typeof(T), type1)
				.Invoke(null, [query, expr2]);
			return (IOrderedQueryable<T>)result1!;
		}

	}

}
