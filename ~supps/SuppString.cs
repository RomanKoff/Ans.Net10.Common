using System.Text;

namespace Ans.Net10.Common
{

	public static class SuppString
	{

		//	https://ru.stackoverflow.com/questions/1387144
		//	Support win1251 and koi8r:
		//		Using System.Text.Encoding.CodePages
		//		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		//	Program.cs ->
		//		SuppIO.Register_CodePagesEncodingProvider();


		/* functions */


		public static string Convert_WINDOWS1251_UTF8(
			string source)
		{
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_WINDOWS1251.GetBytes(source));
		}


		public static string Convert_KOI8R_UTF8(
			string source)
		{
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_KOI8R.GetBytes(source));
		}


		public static string Convert_CP866_UTF8(
			string source)
		{
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_CP866.GetBytes(source));
		}


		public static string Convert_ISO88591_UTF8(
			string source)
		{
			return _Consts.ENCODING_UTF8.GetString(
				_Consts.ENCODING_ISO88591.GetBytes(source));
		}


		public static string GetParamEncode(
			string source)
		{
			return source.ReplaceFromDict(
				new Dictionary<string, string> {
					{ ";", "\\;" },
					{ "=", "\\=" },
					{ "#", "\\#" },
					{ ":", "\\:" }
				});
		}


		public static string GetParamMask(
			string source)
		{
			return source
				.Replace("\\;", _Consts.MASK_semicolon)
				.Replace("\\=", _Consts.MASK_equally)
				.Replace("\\#", _Consts.MASK_sharp)
				.Replace("\\:", _Consts.MASK_colon);
		}


		public static string GetParamDemask(
			string source)
		{
			return source
				.Replace(_Consts.MASK_semicolon, ";")
				.Replace(_Consts.MASK_equally, "=")
				.Replace(_Consts.MASK_sharp, "#")
				.Replace(_Consts.MASK_colon, ":");
		}


		public static string Join(
			string resultTemplate,
			string itemTemplate,
			string separator,
			params string[] items)
		{
			var sb1 = new StringBuilder();
			bool f1 = true;
			bool hasTemplateItem1 = !string.IsNullOrEmpty(itemTemplate);
			foreach (var s1 in items)
				if (!string.IsNullOrEmpty(s1))
				{
					if (f1)
						f1 = false;
					else
						sb1.Append(separator);
					sb1.Append(hasTemplateItem1
						? string.Format(itemTemplate, s1) : s1);
				}
			var s2 = sb1.ToString();
			if (string.IsNullOrEmpty(s2))
				return null;
			return string.IsNullOrEmpty(resultTemplate)
				? s2 : string.Format(resultTemplate, s2);
		}


		public static KeyValuePair<string, string> GetPair(
			string definition)
		{
			int i1 = definition.IndexOf('=');
			if (i1 > 0)
				return new KeyValuePair<string, string>(
					definition[..i1], definition[(i1 + 1)..]);
			return new KeyValuePair<string, string>(
				definition, definition);
		}


		public static string GetFixSpecChars(
			string value)
		{
			var sb1 = new StringBuilder();
			foreach (var char1 in value)
				sb1.Append(
					(char1 < 32) ? ' ' : char1);
			return sb1.ToString();
		}


		public static string GetSafeNumberString(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return null;
			var s1 = SuppLangRu.GetTranslitRuToEn(source.ToLower());
			return s1
				.ReplaceByChars("-", "^0-9a-z")
				.Replace("--", "-")
				.Trim('-');
		}


		public static string GetSafeNameString(
			string source)
		{
			var s1 = SuppLangRu.GetTranslitRuToEn(source.ToLower());
			var sb1 = new StringBuilder();
			for (int i1 = 0; i1 < s1.Length; ++i1)
			{
				char ch1 = s1[i1];
				int code1 = ch1;
				switch (code1)
				{
					case > 47 and < 58: // 0..9
						sb1.Append(ch1);
						break;
					case > 96 and < 123: // a..z
						sb1.Append(ch1);
						break;
					case > 126:
						sb1.Append($"x{code1}");
						break;
					default:
						sb1.Append('_');
						break;
				}
			}
			return sb1.ToString()
				.GetReplaceRecursively("__", "_");
		}


		public static string GetSafeFsString(
			string source)
		{
			var s1 = SuppLangRu.GetTranslitRuToEn(source.ToLower());
			var sb1 = new StringBuilder();
			for (int i1 = 0; i1 < s1.Length; ++i1)
			{
				char ch1 = s1[i1];
				int code1 = ch1;
				switch (code1)
				{
					case > 47 and < 58: // 0..9
						sb1.Append(ch1);
						break;
					case > 96 and < 123: // a..z
						sb1.Append(ch1);
						break;
					case 45 or 94 or 95 or 126: // -^_~
						sb1.Append(ch1);
						break;
					case > 126:
						sb1.Append($"x{code1}");
						break;
					default:
						sb1.Append('_');
						break;
				}
			}
			return sb1.ToString()
				.GetReplaceRecursively("__", "_")
				.GetReplaceRecursively("_-", "-")
				.GetReplaceRecursively("-_", "-")
				.GetReplaceRecursively("--", "-");
		}

	}

}
