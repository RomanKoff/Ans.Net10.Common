namespace Ans.Net10.Common
{

	public class TinyBreakerHelper(
		int step)
	{
		public TinyBreakerHelper()
			: this(1234)
		{
		}

		public int Step { get; } = step;
		public int Current { get; private set; } = 0;

		public bool Next()
		{
			if (Current++ != Step)
				return false;
			Current = 0;
			return true;
		}
	}

}
