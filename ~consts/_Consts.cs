using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace Ans.Net10.Common
{

	public static partial class _Consts
	{

		/*
		 * Support win1251 and koi8r:
		 * Using System.Text.Encoding.CodePages 
		 * Program.cs ->
		 *		SuppIO.Register_CodePagesEncodingProvider();
		 *		// Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		 * https://ru.stackoverflow.com/questions/1387144/%D0%BF%D0%B5%D1%80%D0%B5%D0%B2%D0%BE%D0%B4-%D0%B8%D0%B7-koi8-r-c
		 */


		public static readonly Dictionary<string, string> RTC3339FORMATS
			= new(StringComparer.Ordinal) {
				{ "date", "{0:yyyy-MM-dd}" },
				{ "datetime", @"{0:yyyy-MM-ddTHH\:mm\:ss.fffK}" },
				{ "datetime-local", @"{0:yyyy-MM-ddTHH\:mm\:ss.fff}" },
				{ "time", @"{0:HH\:mm\:ss.fff}" },
			};


		public const string MASK_semicolon = "[x3B]";
		public const string MASK_equally = "[x3D]";
		public const string MASK_sharp = "[x23]";
		public const string MASK_colon = "[x3A]";


		public static readonly char[] SEPS_ARRAY
			= [',', ';'];


		public static readonly Encoding ENCODING_UTF8
			= Encoding.UTF8;

		public static readonly Encoding ENCODING_WINDOWS1251
			= Encoding.GetEncoding(1251);

		public static readonly Encoding ENCODING_KOI8R
			= Encoding.GetEncoding(20866);

		public static readonly Encoding ENCODING_CP866
			= Encoding.GetEncoding(866);

		public static readonly Encoding ENCODING_ISO88591
			= Encoding.GetEncoding("iso-8859-1");


		public static readonly DictString REG_MONTHS
			= new(Resources.Common.Registry_Months);

		public static readonly string[] MONTHS
			= Resources.Common.Array_Months.Split(';');

		public static readonly string[] MONTHS2
			= Resources.Common.Array_Months2.Split(';');


		public static readonly TimeSpan DAY_START_SPAN
			= new(0, 0, 0, 0);

		public static readonly TimeSpan DAY_END_SPAN
			= new(0, 23, 59, 59);


		public const string FORMAT_ANS_DATETIME
			= "yyyy-0MM-dd HH:mmmm:ss";

		public const string FORMAT_ANS_DATE
			= "yyyy-0MM-dd";

		public const string FORMAT_ANS_DATETIME_FILE
			= "yyyy-0MM-dd_HH-mmmm-ss";


		public static readonly MemoryCacheEntryOptions DEFAULT_CACHE_OPTIONS
			= new()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
			};


		public static readonly JsonSerializerOptions JSON_OPTIONS_PNCI
			= new()
			{
				PropertyNameCaseInsensitive = true
			};

	}

}
