// rev 2026-09-21

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ans.Net10.Common.Crud
{

	/// <summary>
	/// Определяет интерфейс универсального репозитория для выполнения базовых
	/// CRUD-операций над сущностями типа <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">Тип доменной сущности, управляемой репозиторием.</typeparam>
	public interface ICrudRepository<T>
		where T : class
	{

		/* readonly properties */


		/// <summary>
		/// Возвращает экземпляр контекста базы данных Entity Framework.
		/// </summary>
		DbContext DbContext { get; }

		/// <summary>
		/// Возвращает набор сущностей <see cref="DbSet{T}"/> для работы с текущим типом данных.
		/// </summary>
		DbSet<T> DbSet { get; }


		/* functions */


		/// <summary>
		/// Формирует запрос <see cref="IQueryable{T}"/> без отслеживания изменений
		/// (AsNoTracking) с применением фильтрации.
		/// </summary>
		/// <param name="filter">Предекатное выражение для фильтрации сущностей.
		/// Если <see langword="null"/>, фильтрация не применяется.</param>
		IQueryable<T> GetItemsAsQueryable(Expression<Func<T, bool>>? filter);

		/// <summary>
		/// Возвращает сущность по её уникальному целочисленному идентификатору.
		/// </summary>
		/// <param name="id">Идентификатор искомой сущности.</param>
		T? GetItem(int id);

		/// <summary>
		/// Возвращает первую сущность, удовлетворяющую условию
		/// фильтра, без отслеживания изменений.
		/// </summary>
		/// <param name="filter">Выражение-фильтр для поиска сущности.</param>
		T? GetItem(Expression<Func<T, bool>> filter);

		/// <summary>
		/// Возвращает общее количество сущностей, удовлетворяющих заданному фильтру.
		/// </summary>
		/// <param name="filter">
		/// Выражение-фильтр для подсчета. Если <see langword="null"/>,
		/// подсчитываются все записи в наборе.
		/// </param>
		int GetItemsCount(Expression<Func<T, bool>>? filter);


		/* methods */


		/// <summary>
		/// Добавляет новую сущность в контекст со статусом <see cref="EntityState.Added"/>.
		/// </summary>
		/// <param name="entity">Добавляемая сущность.</param>
		void Add(T entity);

		/// <summary>
		/// Прикрепляет сущность к контексту и помечает все её свойства
		/// как измененные (<see cref="EntityState.Modified"/>).
		/// </summary>
		/// <param name="entity">Обновляемая сущность.</param>
		void UpdateEvery(T entity);

		/// <summary>
		/// Прикрепляет сущность к контексту и помечает как измененные только указанные свойства.
		/// </summary>
		/// <param name="entity">Обновляемая сущность.</param>
		/// <param name="properties">Коллекция системных имен свойств, которые подлежат обновлению.</param>
		void UpdateSelective(T entity, IEnumerable<string> properties);

		/// <summary>
		/// Помечает указанную сущность для удаления из базы данных.
		/// </summary>
		/// <param name="entity">Удаляемая сущность.</param>
		void Remove(T entity);

		/// <summary>
		/// Находит сущность по идентификатору и помечает её для удаления.
		/// </summary>
		/// <param name="id">Идентификатор удаляемой сущности.</param>
		void Remove(int id);

		/// <summary>
		/// Добавляет связи «многие ко многим» для указанного главного объекта и набора связанных ключей.
		/// </summary>
		/// <param name="masterPtr">Идентификатор главного (владеющего) объекта.</param>
		/// <param name="keys">Коллекция идентификаторов связываемых объектов.</param>
		void AddManyrefs(int masterPtr, IEnumerable<int> keys);

		/// <summary>
		/// Удаляет связи «многие ко многим» для указанного главного объекта и набора связанных ключей.
		/// </summary>
		/// <param name="masterPtr">Идентификатор главного (владеющего) объекта.</param>
		/// <param name="keys">Коллекция идентификаторов отвязываемых объектов.</param>
		void RemoveManyrefs(int masterPtr, IEnumerable<int> keys);

		/// <summary>
		/// Синхронизирует связи «многие ко многим», вычисляя добавленные и удаленные ключи.
		/// </summary>
		/// <param name="masterPtr">Идентификатор главного (владеющего) объекта.</param>
		/// <param name="oldKeys">Старый (текущий) набор связанных ключей.</param>
		/// <param name="newKeys">Новый (целевой) набор связанных ключей.</param>
		void ManyrefUpdate(int masterPtr, IEnumerable<int> oldKeys, IEnumerable<int> newKeys);

	}



	/// <summary>
	/// Базовый класс для реализации репозиториев CRUD-операций с использованием Entity Framework.
	/// </summary>
	/// <typeparam name="T">Тип доменной сущности, управляемой репозиторием.</typeparam>
	public abstract class __CrudRepository_Base<T>(
		DbContext db)
		: ICrudRepository<T>
		where T : class
	{

		/* readonly properties */


		/// <inheritdoc />
		public DbContext DbContext { get; } = db;


		/// <inheritdoc />
		public DbSet<T> DbSet { get; } = db.Set<T>();


		/* functions */


		/// <inheritdoc />
		public virtual IQueryable<T> GetItemsAsQueryable(
			Expression<Func<T, bool>>? filter)
		{
			return filter == null
				? DbSet.AsNoTracking()
				: DbSet.AsNoTracking().Where(filter);
		}


		/// <inheritdoc />
		public virtual T? GetItem(
			int id)
		{
			return DbSet.Find(id);
		}


		/// <inheritdoc />
		public virtual T? GetItem(
			Expression<Func<T, bool>> filter)
		{
			return DbSet
				.AsNoTracking()
				.FirstOrDefault(filter);
		}


		/// <inheritdoc />
		public virtual int GetItemsCount(
			Expression<Func<T, bool>>? filter)
		{
			return filter == null
				? DbSet.Count()
				: DbSet.Where(filter).Count();
		}


		/* methods */


		/// <inheritdoc />
		public virtual void Add(
			T entity)
		{
			DbSet.Add(entity);
		}


		/// <inheritdoc />
		public virtual void UpdateEvery(
			T entity)
		{
			DbSet.Attach(entity);
			DbContext.Entry(entity).State = EntityState.Modified;
		}


		/// <inheritdoc />
		public virtual void UpdateSelective(
			T entity,
			IEnumerable<string> properties)
		{
			DbSet.Attach(entity);
			foreach (var property1 in properties)
				DbContext.Entry(entity).Property(property1).IsModified = true;
		}


		/// <inheritdoc />
		public virtual void Remove(
			T entity)
		{
			if (DbContext.Entry(entity).State == EntityState.Detached)
				DbSet.Attach(entity);
			DbSet.Remove(entity);
		}


		/// <inheritdoc />
		public virtual void Remove(
			int id)
		{
			var entity1 = DbSet.Find(id);
			if (entity1 != null)
				Remove(entity1);
		}


		/// <inheritdoc />
		public virtual void AddManyrefs(
			int masterPtr,
			IEnumerable<int> keys)
		{
			throw new NotImplementedException();
		}


		/// <inheritdoc />
		public virtual void RemoveManyrefs(
			int masterPtr,
			IEnumerable<int> keys)
		{
			throw new NotImplementedException();
		}


		/// <inheritdoc />
		public virtual void ManyrefUpdate(
			int masterPtr,
			IEnumerable<int> oldKeys,
			IEnumerable<int> newKeys)
		{
			var comparer1 = new KeysComparer(oldKeys, newKeys);
			if (comparer1.HasAdded)
				AddManyrefs(masterPtr, comparer1.Added);
			if (comparer1.HasDeleted)
				RemoveManyrefs(masterPtr, comparer1.Deleted);
		}

	}

}
