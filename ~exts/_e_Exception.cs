namespace Ans.Net10.Common
{

	public static partial class _e_Exception
	{

		/* functions */


		public static string GetExceptionMessage(
			this Exception exception)
		{
			return (exception.InnerException == null)
				? exception.Message
				: exception.InnerException.GetExceptionMessage();
		}


		public static bool TestContains(
			this Exception exception,
			string value)
		{
			if (exception.Message.Contains(value))
				return true;
			return exception.InnerException != null
				&& exception.InnerException.TestContains(value);
		}


		public static bool TestStartsWith(
			this Exception exception,
			string value)
		{
			if (exception.Message.StartsWith(value))
				return true;
			return exception.InnerException != null
				&& exception.InnerException.TestContains(value);
		}

	}

}