namespace Ans.Net10.Common
{

	public static class SuppCrud
	{

		/* functions */


		public static string _FixTextInline(
			string value)
		{
			return SuppTypograph.GetFixTextLine(value);
		}


		public static string _FixTextBlock(
			string value)
		{
			return SuppTypograph.GetFixTextBox(value);
		}


		public static string Text50_Fix(string value) => _FixTextInline(value);
		public static string Text100_Fix(string value) => _FixTextInline(value);
		public static string Text250_Fix(string value) => _FixTextInline(value);
		public static string Text400_Fix(string value) => _FixTextInline(value);

		public static string Memo_Fix(string value) => _FixTextBlock(value);
		public static string Name_Fix(string value) => _FixTextInline(value);
		public static string Varname_Fix(string value) => _FixTextInline(value);
		public static string Email_Fix(string value) => _FixTextInline(value);

		public static int Int_Fix(int value) => value;
		public static int? Int_Fix(int? value) => value;
		public static long Long_Fix(long value) => value;
		public static long? Long_Fix(long? value) => value;
		public static float Float_Fix(float value) => value;
		public static float? Float_Fix(float? value) => value;
		public static double Double_Fix(double value) => value;
		public static double? Double_Fix(double? value) => value;
		public static decimal Decimal_Fix(decimal value) => value;
		public static decimal? Decimal_Fix(decimal? value) => value;

		public static DateTime? DateTime_Fix(DateTime? value) => value;
		public static DateOnly? DateOnly_Fix(DateOnly? value) => value;
		public static TimeOnly? TimeOnly_Fix(TimeOnly? value) => value;

		public static bool Bool_Fix(bool value) => value;
		public static int Enum_Fix(int value) => value;
		public static string Set_Fix(string value) => value;
		public static int Reference_Fix(int value) => value;
		public static int? Reference_Fix(int? value) => value;


		/* custom */


		public static string ToUpper_Fix(
			string value)
		{
			return value?.ToUpper();
		}


		public static string TextInline_Fix(
			string value)
		{
			return _FixTextInline(value).GetFirstUpper(false);
		}


		public static string HtmlArticle_BeforeSave(
			string value)
		{
			return value?
				.Replace(
					"<figure class=\"table\"><table>",
					"<div class=\"table-responsive\"><table class=\"table table-bordered\">")
				.Replace(
					"</table></figure>",
					"</table></div>")
				.Replace(
					"<p>&nbsp;</p>",
					"")
				.Replace(
					"<p>.</p>",
					"")
				.Trim();
		}


		public static string HtmlArticle_BeforeEdit(
			string value)
		{
			return value?
				.Replace(
					"<div class=\"table-responsive\"><table class=\"table table-bordered\">",
					"<figure class=\"table\"><table>")
				.Replace(
					"</table></div>",
					"</table></figure>")
				.Trim();
		}

	}

}
