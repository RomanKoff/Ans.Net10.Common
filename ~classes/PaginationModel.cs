namespace Ans.Net10.Common
{

	public class PaginationModel
	{

		/* ctor */


		public PaginationModel()
		{
		}


		public PaginationModel(
			PaginationHelper source)
			: this()
		{
			CurrentPage = source.CurrentPage;
			TotalItems = source.TotalItems;
			TotalPages = source.TotalPages;
			SkipItems = source.SkipItems;
			ItemsOnPage = source.ItemsOnPage;

			StartPage = source.StartPage;
			PreviousPage = source.PreviousPage;
			NextPage = source.NextPage;
			EndPage = source.EndPage;

			ActiveFirstPage = source.ActiveFirstPage;
			ActiveLastPage = source.ActiveLastPage;
			HasItemsBefore = source.HasItemsBefore;
			HasItemsAfter = source.HasItemsAfter;

			NotValidIndex = source.NotValidIndex;
			Offset = source.Offset;
		}


		/* properties */


		public int CurrentPage { get; }
		public int TotalItems { get; }
		public int TotalPages { get; }
		public int SkipItems { get; }
		public int ItemsOnPage { get; }

		public int StartPage { get; }
		public int PreviousPage { get; }
		public int NextPage { get; }
		public int EndPage { get; }

		public bool ActiveFirstPage { get; }
		public bool ActiveLastPage { get; }
		public bool HasItemsBefore { get; }
		public bool HasItemsAfter { get; }

		public bool NotValidIndex { get; }
		public int Offset { get; }

	}

}
