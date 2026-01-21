namespace Ans.Net10.Common
{

	public class TinyTimerHelper
	{

		/* ctor */


		public TinyTimerHelper(
			TimeSpan span,
			bool useEqualIntervals)
		{
			Span = span;
			NextStop = DateTime.Now + Span;
			UseEqualIntervals = useEqualIntervals;
		}


		/* properties */


		public TimeSpan Span { get; set; }
		public DateTime NextStop { get; set; }
		public bool UseEqualIntervals { get; set; }


		/* functions */


		public bool Test()
		{
			if (NextStop < DateTime.Now)
			{
				NextStop = (UseEqualIntervals)
					? NextStop + Span
					: DateTime.Now + Span;
				return true;
			}
			return false;
		}

	}

}
