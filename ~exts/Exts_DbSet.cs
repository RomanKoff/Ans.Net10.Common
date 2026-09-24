// rev 2026-09-21

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Предоставляет методы расширения для работы с объектами <see cref="DbSet{TEntity}"/>.
	/// </summary>
	public static partial class Exts_DbSet
	{

		/// <summary>
		/// Возвращает экземпляр <see cref="DbContext"/>, к которому
		/// принадлежит данный набор <see cref="DbSet{TEntity}"/>.
		/// </summary>
		/// <typeparam name="TEntity">Тип доменной сущности, управляемой набором данных.</typeparam>
		/// <param name="dbSet">Текущий экземпляр набора данных.</param>
		/// <returns>
		/// Экземпляр <see cref="DbContext"/> или <see langword="null"/>,
		/// если контекст не удалось извлечь из инфраструктуры.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DbContext? GetDbContext<TEntity>(
			this DbSet<TEntity> dbSet)
			where TEntity : class
		{
			var infrastructure1 = dbSet as IInfrastructure<IServiceProvider>;
			var provider1 = infrastructure1?.Instance;
			var currentDbContext1 = provider1?.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
			return currentDbContext1?.Context;
		}

	}

}
