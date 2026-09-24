// rev 2026-09-18

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Хелпер для расчёта диапазонов страниц, смещений (Skip/Take)
	/// и навигационных состояний интерфейса пагинации.
	/// </summary>
	public class PaginationHelper
	{

		private int _currentPage = 1;


		/* ctors */


		/// <summary>
		/// Инициализирует новый экземпляр хелпера пагинации и сразу вычисляет все навигационные свойства.
		/// </summary>
		/// <param name="itemsOnPage">Количество отображаемых элементов на одной странице.</param>
		/// <param name="totalItems">Общее количество элементов в коллекции.</param>
		/// <param name="currentPage">Номер текущей активной страницы (по умолчанию 1).</param>
		/// <param name="offset">Радиус отображения соседних страниц вокруг текущей (от 1 до 9, по умолчанию 4).</param>
		public PaginationHelper(
			int itemsOnPage,
			int totalItems,
			int currentPage = 1,
			int offset = 4)
		{
			if (itemsOnPage < 1)
				itemsOnPage = 1;
			if (totalItems < 0)
				totalItems = 0;
			Offset = SuppMath.GetRestrict(offset, 1, 9);
			ItemsOnPage = itemsOnPage;
			TotalItems = totalItems;
			TotalPages = (int)Math.Ceiling(TotalItems / (double)ItemsOnPage);
			CurrentPage = currentPage;
		}


		/* properties */


		/// <summary>
		/// Возвращает или задает номер текущей активной страницы. 
		/// При установке автоматически пересчитывает все связанные навигационные свойства.
		/// </summary>
		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				_currentPage = value;
				if (_currentPage < 1)
				{
					_currentPage = 1;
					NotValidIndex = true;
				}
				if (TotalPages > 0 && _currentPage > TotalPages)
				{
					_currentPage = TotalPages;
					NotValidIndex = true;
				}
				SkipItems = ItemsOnPage * (_currentPage - 1);
				if (_currentPage < Offset + 1)
				{
					StartPage = 1;
					EndPage = Offset * 2 + 1;
				}
				else if (_currentPage > TotalPages - Offset - 1)
				{
					StartPage = TotalPages - Offset * 2;
					EndPage = TotalPages;
				}
				else
				{
					StartPage = _currentPage - Offset;
					EndPage = _currentPage + Offset;
				}
				if (StartPage < 1)
					StartPage = 1;
				if (EndPage > TotalPages)
					EndPage = TotalPages;
				ActiveFirstPage = (_currentPage == 1);
				ActiveLastPage = (_currentPage == TotalPages);
				HasItemsBefore = (StartPage > 1);
				HasItemsAfter = (EndPage < TotalPages);
				PreviousPage = _currentPage - (2 * Offset);
				if (PreviousPage < 1)
					PreviousPage = 1;
				NextPage = _currentPage + (2 * Offset);
				if (NextPage > TotalPages)
					NextPage = TotalPages;
			}
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает количество элементов на одной странице.
		/// </summary>
		public int ItemsOnPage { get; }


		/// <summary>
		/// Возвращает радиус отображения соседних страниц в блоке навигации.
		/// </summary>
		public int Offset { get; }


		/// <summary>
		/// Возвращает общее количество элементов в коллекции.
		/// </summary>
		public int TotalItems { get; }


		/// <summary>
		/// Возвращает общее рассчитанное количество страниц.
		/// </summary>
		public int TotalPages { get; }


		/// <summary>
		/// Возвращает количество пропущенных элементов для SQL/LINQ запроса (параметр Skip).
		/// </summary>
		public int SkipItems { get; private set; }


		/// <summary>
		/// Возвращает номер страницы для быстрого блочного перехода назад.
		/// </summary>
		public int PreviousPage { get; private set; }


		/// <summary>
		/// Возвращает номер страницы для быстрого блочного перехода вперед.
		/// </summary>
		public int NextPage { get; private set; }


		/// <summary>
		/// Возвращает начальный номер страницы в текущем отображаемом диапазоне.
		/// </summary>
		public int StartPage { get; private set; }


		/// <summary>
		/// Возвращает конечный номер страницы в текущем отображаемом диапазоне.
		/// </summary>
		public int EndPage { get; private set; }


		/// <summary>
		/// Возвращает признак того, является ли текущая страница первой.
		/// </summary>
		public bool ActiveFirstPage { get; private set; }


		/// <summary>
		/// Возвращает признак того, является ли текущая страница последней.
		/// </summary>
		public bool ActiveLastPage { get; private set; }


		/// <summary>
		/// Возвращает признак наличия страниц до текущего отображаемого диапазона (для вывода многоточия "...").
		/// </summary>
		public bool HasItemsBefore { get; private set; }


		/// <summary>
		/// Возвращает признак наличия страниц после текущего отображаемого диапазона (для вывода многоточия "...").
		/// </summary>
		public bool HasItemsAfter { get; private set; }


		/// <summary>
		/// Возвращает <see langword="true"/>, если переданный изначально номер страницы вышел за допустимые пределы и был скорректирован.
		/// </summary>
		public bool NotValidIndex { get; private set; }


		/* functions */


		/// <summary>
		/// Формирует текстовую диагностическую информацию о текущем состоянии пагинатора.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString()
		{
			var before1 = HasItemsBefore ? $"🡠{PreviousPage} " : string.Empty;
			var after1 = HasItemsAfter ? $" {NextPage}🡢" : string.Empty;
			return $"Items: {ItemsOnPage}/{TotalItems}  Pages: {TotalPages} {before1}[{StartPage}-{CurrentPage}-{EndPage}]{after1}  FirstPage:{ActiveFirstPage}  LastPage:{ActiveLastPage}  Skip:{SkipItems}";
		}

	}

}
