// rev 2026-09-18

namespace Ans.Net10.Common
{

	/// <summary>
	/// Неизменяемая структура-модель данных пагинации для передачи состояния на интерфейсный слой.
	/// </summary>
	public readonly record struct PaginationModel(
		int CurrentPage,
		int TotalItems,
		int TotalPages,
		int SkipItems,
		int ItemsOnPage,
		int StartPage,
		int PreviousPage,
		int NextPage,
		int EndPage,
		bool ActiveFirstPage,
		bool ActiveLastPage,
		bool HasItemsBefore,
		bool HasItemsAfter,
		bool NotValidIndex,
		int Offset)
	{

		/// <summary>
		/// Инициализирует модель пагинации на основе вычисленного состояния хелпера.
		/// </summary>
		/// <param name="source">Экземпляр хелпера пагинации.</param>
		public PaginationModel(
			PaginationHelper source)
			: this(
				source.CurrentPage,
				source.TotalItems,
				source.TotalPages,
				source.SkipItems,
				source.ItemsOnPage,
				source.StartPage,
				source.PreviousPage,
				source.NextPage,
				source.EndPage,
				source.ActiveFirstPage,
				source.ActiveLastPage,
				source.HasItemsBefore,
				source.HasItemsAfter,
				source.NotValidIndex,
				source.Offset)
		{
		}

	}

}
