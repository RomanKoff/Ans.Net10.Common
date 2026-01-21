using System.Text;

namespace Ans.Net10.Common
{

	public class PaginationHelper
	{

		/* ctors */


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
		/// Возвращает и устанавливает текущую страницу
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
				PreviousPage = CurrentPage - (2 * Offset);
				if (PreviousPage < 1)
					PreviousPage = 1;
				NextPage = CurrentPage + (2 * Offset);
				if (NextPage > TotalPages)
					NextPage = TotalPages;
			}
		}
		private int _currentPage = 1;


		/* readonly properties */


		/// <summary>
		/// Элементов на странице
		/// </summary>
		public int ItemsOnPage { get; }

		/// <summary>
		/// Смещение от границ пагинатора
		/// </summary>
		public int Offset { get; }

		/// <summary>
		/// Всего элементов
		/// </summary>
		public int TotalItems { get; }

		/// <summary>
		/// Всего страниц
		/// </summary>
		public int TotalPages { get; }


		/// <summary>
		/// Пропущего элементов
		/// </summary>
		public int SkipItems { get; private set; }

		/// <summary>
		/// Предыдущая страница пагинатора
		/// </summary>
		public int PreviousPage { get; private set; }

		/// <summary>
		/// Следующая страница пагинатора
		/// </summary>
		public int NextPage { get; private set; }

		/// <summary>
		/// Стартовая страница пагинатора
		/// </summary>
		public int StartPage { get; private set; }

		/// <summary>
		/// Конечная страница пагинатора
		/// </summary>
		public int EndPage { get; private set; }

		/// <summary>
		/// Признак активности первой страницы
		/// </summary>
		public bool ActiveFirstPage { get; private set; }

		/// <summary>
		/// Признак активности последней страницы
		/// </summary>
		public bool ActiveLastPage { get; private set; }

		/// <summary>
		/// Есть элементы до
		/// </summary>
		public bool HasItemsBefore { get; private set; }

		/// <summary>
		/// Есть элементы после
		/// </summary>
		public bool HasItemsAfter { get; private set; }

		/// <summary>
		/// Некорректный индекс
		/// </summary>
		public bool NotValidIndex { get; private set; }


		/* functions */


		public string GetInfo()
		{
			var sb1 = new StringBuilder();
			sb1.Append($"Items: {ItemsOnPage}/{TotalItems} ");
			sb1.Append($"Pages: {TotalPages} ");
			if (HasItemsBefore)
				sb1.Append($"🡠{PreviousPage} ");
			sb1.Append($"[{StartPage}-{CurrentPage}-{EndPage}]");
			if (HasItemsAfter)
				sb1.Append($" {NextPage}🡢");
			sb1.Append($" FirstPage:{ActiveFirstPage} LastPage:{ActiveLastPage} Skip:{SkipItems}");
			return sb1.ToString();
		}

	}

}