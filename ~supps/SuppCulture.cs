using System.Globalization;

namespace Ans.Net10.Common
{

	public static class SuppCulture
	{

		/* methods */


		public static void SetCulture(
			CultureInfo culture)
		{
			Thread.CurrentThread.CurrentUICulture = culture;
			Thread.CurrentThread.CurrentCulture = CultureInfo
				.CreateSpecificCulture(culture.Name);
		}


		public static void SetCulture(
			string culture)
		{
			SetCulture(new CultureInfo(culture));
		}

	}

}
