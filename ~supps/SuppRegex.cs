namespace Ans.Net10.Common
{

	public static class SuppRegex
	{

		/* functions */


		/// <summary>
		/// Кодировка спецсимволов для regex-выражений
		/// </summary>
		public static string ParamEncode(
			string source)
		{
			return _Consts.G_REGEX_ENCODE().Replace(source, @"\$&");
		}

	}

}
