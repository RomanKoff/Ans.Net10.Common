using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ans.Net10.Common.Crud
{

	public interface ISlaveEntity
		: IMasterEntity
	{
		int MasterPtr { get; set; }
	}



	public interface ICrudSlaveRepository<T>
		: ICrudRepository<T>
		where T : class, ISlaveEntity
	{
		T GetNew(int masterPtr);
		IQueryable<T> GetItemsAsQueryable(
			int masterPtr,
			Expression<Func<T, bool>> filter);
		int GetItemsCount(int masterPtr);
	}



	public abstract class _CrudSlaveRepository_Proto<T>(
		DbContext db)
		: __CrudRepository_Base<T>(db),
		ICrudSlaveRepository<T>
		where T : class, ISlaveEntity
	{

		/* functions */


		public abstract T GetNew(
			int masterPtr);


		public virtual IQueryable<T> GetItemsAsQueryable(
			int masterPtr,
			Expression<Func<T, bool>> filter)
		{
			if (filter == null)
				filter = x => x.MasterPtr == masterPtr;
			else
				filter.And(x => x.MasterPtr == masterPtr);
			var query1 = filter == null
				? DbSet.AsQueryable()
				: DbSet.Where(filter);
			return query1;
		}


		public virtual int GetItemsCount(
			int masterPtr)
		{
			return GetItemsCount(x => x.MasterPtr == masterPtr);
		}

	}

}
