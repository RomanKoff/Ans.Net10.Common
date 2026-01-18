using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ans.Net10.Common.Crud
{

	public interface ICrudRepository<T>
		where T : class
	{
		DbContext DbContext { get; }
		DbSet<T> DbSet { get; }

		IQueryable<T> GetItemsAsQueryable(
			Expression<Func<T, bool>> filter);
		T GetItem(int id);
		T GetItem(Expression<Func<T, bool>> filter);
		int GetItemsCount(Expression<Func<T, bool>> filter);

		void Add(T entity);
		void UpdateEvery(T entity);
		void UpdateSelective(
			T entity,
			IEnumerable<string> properties);
		void Remove(T entity);
		void Remove(int id);

		void AddManyrefs(int masterPtr, IEnumerable<int> keys);
		void RemoveManyrefs(int masterPtr, IEnumerable<int> keys);

		void ManyrefUpdate(int masterPtr, IEnumerable<int> oldKeys, IEnumerable<int> newKeys);
	}



	public class __CrudRepository_Base<T>(
		DbContext db)
		: ICrudRepository<T>
		where T : class
	{

		/* readonly properties */


		public DbContext DbContext { get; } = db;
		public DbSet<T> DbSet { get; } = db.Set<T>();


		/* functions */


		public virtual IQueryable<T> GetItemsAsQueryable(
			Expression<Func<T, bool>> filter)
		{
			var query1 = filter == null
				? DbSet.AsNoTracking()
				: DbSet.AsNoTracking().Where(filter);
			return query1;
		}


		public virtual T GetItem(
			int id)
		{
			var item1 = DbSet.Find(id);
			return item1;
		}


		public virtual T GetItem(
			Expression<Func<T, bool>> filter)
		{
			var item1 = DbSet
				.AsNoTracking()
				.FirstOrDefault(filter);
			return item1;
		}


		public virtual int GetItemsCount(
			Expression<Func<T, bool>> filter)
		{
			return DbSet.Where(filter).Count();
		}


		/* methods */


		public virtual void Add(
			T entity)
		{
			DbSet.Add(entity);
		}


		public virtual void UpdateEvery(
			T entity)
		{
			DbSet.Attach(entity);
			DbContext.Entry(entity).State = EntityState.Modified;
		}


		public virtual void UpdateSelective(
			T entity,
			IEnumerable<string> properties)
		{
			DbSet.Attach(entity);
			foreach (var item1 in properties)
				DbSet.Entry(entity).Property(item1).IsModified = true;
		}


		public virtual void Remove(
			T entity)
		{
			if (DbContext.Entry(entity).State == EntityState.Detached)
				DbSet.Attach(entity);
			DbSet.Remove(entity);
		}


		public virtual void Remove(
			int id)
		{
			var entity1 = DbSet.Find(id);
			Remove(entity1);
		}


		public virtual void AddManyrefs(
			int masterPtr,
			IEnumerable<int> keys)
		{
			throw new NotImplementedException();
		}


		public virtual void RemoveManyrefs(
			int masterPtr,
			IEnumerable<int> keys)
		{
			throw new NotImplementedException();
		}


		public virtual void ManyrefUpdate(
			int masterPtr,
			IEnumerable<int> oldKeys,
			IEnumerable<int> newKeys)
		{
			throw new NotImplementedException();
		}

	}

}
