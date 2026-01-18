namespace Ans.Net10.Common
{

	public abstract class _AppSettingsOptions_Proto
	{

		/* abstracts */


		public abstract void Test();


		/* properties */


		public string SectionName { get; set; }


		/* functions */


		public Exception GetExceptionParamRequired(
			string paramName)
		{
			return new Exception(
				$"[appsettings.json/{SectionName}/{paramName}] is required!");
		}

	}

}
