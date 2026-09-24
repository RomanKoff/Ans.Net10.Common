// rev 2026-09-

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Определяет интерфейс для элементов древовидной структуры.
	/// </summary>
	public interface ITreeItem
	{
		/* properties */

		/// <summary>
		/// Получает или задает родительский элемент текущего узла.
		/// </summary>
		ITreeItem? Parent { get; set; }

		/* readonly properties */

		/// <summary>
		/// Получает перечисление всех вышестоящих родительских элементов до корня дерева.
		/// </summary>
		IEnumerable<ITreeItem> Parents { get; }

		/// <summary>
		/// Получает перечисление прямых дочерних элементов текущего узла.
		/// </summary>
		IEnumerable<ITreeItem> Children { get; }

		/// <summary>
		/// Возвращает признак наличия родительского элемента.
		/// </summary>
		bool HasParent { get; }

		/// <summary>
		/// Возвращает признак наличия дочерних элементов.
		/// </summary>
		bool HasChildren { get; }

		/* methods */

		/// <summary>
		/// Добавляет дочерний узел в конец списка текущего элемента.
		/// </summary>
		/// <param name="item">Добавляемый дочерний элемент.</param>
		void AppendChild(ITreeItem item);

		/// <summary>
		/// Массово добавляет дочерние узлы в конец списка текущего элемента.
		/// </summary>
		/// <param name="items">Массив добавляемых дочерних элементов.</param>
		void AppendChildren(params ITreeItem[] items);

		/* functions */

		/// <summary>
		/// Выполняет рекурсивный поиск первого элемента заданного типа, удовлетворяющего условию предиката.
		/// </summary>
		/// <typeparam name="T">Целевой тип искомого объекта.</typeparam>
		/// <param name="func">Функция-предикат для проверки условий.</param>
		/// <returns>Найденный объект типа <typeparamref name="T"/> или значение по умолчанию.</returns>
		T? FindItem<T>(Func<T, bool> func) where T : class;
	}



	/// <summary>
	/// Базовая реализация элемента древовидной структуры.
	/// </summary>
	public class _TreeItem_Base
		: ITreeItem
	{

		private List<ITreeItem>? _children;


		/* properties */


		/// <inheritdoc />
		public ITreeItem? Parent { get; set; }


		/* readonly properties */


		/// <inheritdoc />
		public IEnumerable<ITreeItem> Parents
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _getParents();
		}


		/// <inheritdoc />
		public IEnumerable<ITreeItem> Children
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _children ?? Enumerable.Empty<ITreeItem>();
		}


		/// <inheritdoc />
		public bool HasChildren
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _children?.Count > 0;
		}


		/// <inheritdoc />
		public bool HasParent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Parent != null;
		}


		/* methods */


		/// <inheritdoc />
		/// <exception cref="ArgumentException">
		/// Вызывается, если объект пытается добавить самого себя в качестве дочернего узла.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Вызывается при обнаружении циклической зависимости в иерархии дерева.
		/// </exception>
		public void AppendChild(
			ITreeItem item)
		{
			if (item == null)
				return;
			if (item == this)
				throw new ArgumentException(
					"[Ans.Net10.Common] An object cannot be its own Child.");
			var temp1 = Parent;
			while (temp1 != null)
			{
				if (temp1 == item)
					throw new InvalidOperationException(
						"[Ans.Net10.Common] The detected loop: this object is already a Parent in the chain above, the object cannot be its own Child.");
				temp1 = temp1.Parent;
			}
			_children ??= [];
			_children.Add(item);
			item.Parent = this;
		}


		/// <inheritdoc />
		public void AppendChildren(
			params ITreeItem[] items)
		{
			if (items == null)
				return;
			for (int i1 = 0; i1 < items.Length; i1++)
				if (items[i1] != null)
					AppendChild(items[i1]);
		}


		/* functions */


		/// <inheritdoc />
		public T? FindItem<T>(
			Func<T, bool> func)
			where T : class
		{
			if (func == null
				|| _children == null
				|| _children.Count == 0)
				return null;
			for (int i1 = 0; i1 < _children.Count; i1++)
			{
				var current1 = _children[i1];
				if (current1 is T typedItem1 && func(typedItem1))
					return typedItem1;
				var foundInside1 = current1.FindItem(func);
				if (foundInside1 != null)
					return foundInside1;
			}
			return null;
		}


		/* privates */


		private IEnumerable<ITreeItem> _getParents()
		{
			var current1 = Parent;
			while (current1 != null)
			{
				yield return current1;
				current1 = current1.Parent;
			}
		}

	}

}
