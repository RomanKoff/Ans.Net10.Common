using Microsoft.EntityFrameworkCore;

namespace Ans.Net10.Common.Crud
{

	public interface IMasterEntity
	{
		int Id { get; set; }
	}



	public interface ICrudMasterRepository<T>
		: ICrudRepository<T>
		where T : class, IMasterEntity
	{
		T GetNew();
		int GetItemsCount();
	}



	public abstract class _CrudMasterRepository_Proto<T>(
		DbContext db)
		: __CrudRepository_Base<T>(db),
		ICrudMasterRepository<T>
		where T : class, IMasterEntity
	{

		/* functions */


		public abstract T GetNew();


		public virtual int GetItemsCount()
		{
			return DbSet.Count();
		}

	}

}
