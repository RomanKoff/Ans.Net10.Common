using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ans.Net10.Common
{

	public static partial class _e_DbSet
	{

		/* functions */


		public static DbContext GetDbContext<TEntity>(
			this DbSet<TEntity> dbSet)
			where TEntity : class
		{
			var infrastructure1 = dbSet as IInfrastructure<IServiceProvider>;
			var provider1 = infrastructure1.Instance;
			var context1 = provider1.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
			return context1.Context;
		}

	}

}
