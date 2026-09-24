// rev 2026-09-21

namespace Ans.Net10.Common
{

	/// <summary>
	/// Определяет интерфейс сущности, поддерживающей древовидную (иерархическую) структуру.
	/// </summary>
	public interface ITreeEntity
	{
		/// <summary>
		/// Уникальный идентификатор сущности.
		/// </summary>
		int Id { get; set; }

		/// <summary>
		/// Идентификатор родительской сущности. Равен <see langword="null"/> для корневых элементов.
		/// </summary>
		int? ParentPtr { get; set; }

		/// <summary>
		/// Порядковый номер для сортировки элементов на одном уровне вложенности.
		/// </summary>
		int Order { get; set; }
	}



	/// <summary>
	/// Обертка над сущностью древовидной структуры, содержащая вычисленные метаданные иерархии.
	/// </summary>
	/// <typeparam name="TEntity">Тип исходной доменной сущности, реализующей <see cref="ITreeEntity"/>.</typeparam>
	public class TreeEntityWrapper<TEntity>
		where TEntity : class, ITreeEntity
	{
		/// <summary>
		/// Уникальный идентификатор сущности.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Идентификатор родительской сущности.
		/// </summary>
		public int? ParentPtr { get; set; }

		/// <summary>
		/// Порядковый номер для сортировки.
		/// </summary>
		public int Order { get; set; }

		/// <summary>
		/// Вычисленный уровень вложенности в дереве (индексация с 0 для корня).
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Ссылка на родительский узел-обертку.
		/// </summary>
		public TreeEntityWrapper<TEntity>? Parent { get; set; }

		/// <summary>
		/// Коллекция прямых дочерних узлов-оберток.
		/// </summary>
		public IEnumerable<TreeEntityWrapper<TEntity>>? Children { get; set; }

		/// <summary>
		/// Ссылка на оригинальный экземпляр доменной сущности.
		/// </summary>
		public TEntity? Value { get; set; }
	}



	/// <summary>
	/// Вспомогательный класс для построения, индексации и линеаризации иерархических
	/// древовидных структур из плоских коллекций.
	/// </summary>
	/// <typeparam name="TEntity">Тип доменной сущности, реализующей <see cref="ITreeEntity"/>.</typeparam>
	public class TreeEntityHelper<TEntity>
		where TEntity : class, ITreeEntity
	{

		private readonly IEnumerable<TEntity> _source;
		private readonly Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> _funcOrder;

		private readonly List<TreeEntityWrapper<TEntity>> _topItems = [];
		private readonly List<TreeEntityWrapper<TEntity>> _allItems = [];


		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="TreeEntityHelper{TEntity}"/>
		/// и запускает рекурсивное построение дерева.
		/// </summary>
		/// <param name="source">Исходная плоская коллекция доменных сущностей.</param>
		/// <param name="currentId">
		/// Опциональный идентификатор элемента, который (вместе со своим поддеревом) будет исключен из построения.
		/// </param>
		/// <param name="funcOrder">
		/// Делегат, задающий правила сортировки элементов на каждом уровне дерева.
		/// </param>
		public TreeEntityHelper(
			IEnumerable<TEntity> source,
			int? currentId,
			Func<IEnumerable<TEntity>, IOrderedEnumerable<TEntity>> funcOrder)
		{
			_source = source;
			_funcOrder = funcOrder;
			_scanTable(null, 0, currentId);
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает коллекцию корневых (наивысших) элементов построенного дерева.
		/// </summary>
		public IEnumerable<TreeEntityWrapper<TEntity>> TopItems
			=> _topItems;


		/// <summary>
		/// Возвращает плоский линеаризованный список всех элементов дерева в порядке их обхода (Pre-order обоход).
		/// </summary>
		public IEnumerable<TreeEntityWrapper<TEntity>> AllItems
			=> _allItems;


		/* privates */


		private void _scanTable(
			TreeEntityWrapper<TEntity>? parent,
			int level,
			int? currentId)
		{
			var items1 = _funcOrder(
				_source.Where(x => x.ParentPtr == parent?.Value?.Id && x.Id != currentId));
			var children1 = new List<TreeEntityWrapper<TEntity>>();
			foreach (var item1 in items1)
			{
				var item2 = new TreeEntityWrapper<TEntity>
				{
					Id = item1.Id,
					ParentPtr = item1.ParentPtr,
					Parent = parent,
					Order = item1.Order,
					Level = level,
					Value = item1
				};
				if (parent == null)
					_topItems.Add(item2);
				else
					children1.Add(item2);

				_allItems.Add(item2);
				_scanTable(item2, level + 1, currentId);
			}
			if (children1.Count > 0)
				parent?.Children = children1;
		}
	}

}
