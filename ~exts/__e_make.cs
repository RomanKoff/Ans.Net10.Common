namespace Ans.Net10.Common
{

	public static partial class __e_make
	{

		// string


		public static string Make(
			this string value,
			string template)
		{
			return string.IsNullOrEmpty(value)
				? null : string.IsNullOrEmpty(template)
					? value : string.Format(template, value);
		}


		public static string Make(
			this string value,
			string template,
			string nullValue)
		{
			return value == nullValue
				? null : string.Format(template, value);
		}


		public static string MakeMore(
			this string value,
			string template,
			params object[] args)
		{
			return string.IsNullOrEmpty(value)
				? null : string.IsNullOrEmpty(template)
					? value : string.Format(template, args.GetArrayInsert(0, value));
		}


		// int


		public static string Make(
			this int value,
			string template,
			string format,
			int nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}


		public static string Make(
			this int value,
			string template,
			string format = null)
		{
			return value.Make(template, format, 0);
		}
		
		
		public static string Make(
			this int? value,
			string template,
			string format = null)
		{
			return value?.Make(format, template);
		}


		// long


		public static string Make(
			this long value,
			string template,
			string format,
			long nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}
		
		
		public static string Make(
			this long value,
			string template,
			string format = null)
		{
			return value.Make(template, format, 0);
		}
		
		
		public static string Make(
			this long? value,
			string template,
			string format = null)
		{
			return value?.Make(format, template);
		}


		// double


		public static string Make(
			this double value,
			string template,
			string format,
			double nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}
		
		
		public static string Make(
			this double value,
			string template,
			string format = null)
		{
			return value.Make(template, format, 0);
		}
		
		
		public static string Make(
			this double? value,
			string template,
			string format = null)
		{
			return value?.Make(format, template);
		}


		// float


		public static string Make(
			this float value,
			string template,
			string format,
			float nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}
		
		
		public static string Make(
			this float value,
			string template,
			string format = null)
		{
			return value.Make(template, format, 0);
		}
		
		
		public static string Make(
			this float? value,
			string template,
			string format = null)
		{
			return value?.Make(format, template);
		}


		// decimal


		public static string Make(
			this decimal value,
			string template,
			string format,
			decimal nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}
		
		
		public static string Make(
			this decimal value,
			string template,
			string format = null)
		{
			return value.Make(template, format, 0);
		}
		
		
		public static string Make(
			this decimal? value,
			string template,
			string format = null)
		{
			return value?.Make(format, template);
		}


		// DateTime


		public static string Make(
			this DateTime value,
			string template,
			string format,
			DateTime nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}
		
		
		public static string Make(
			this DateTime value,
			string template,
			string format = null)
		{
			return value.Make(template, format, DateTime.MinValue);
		}
		
		
		public static string Make(
			this DateTime? value,
			string template,
			string format = null)
		{
			return value?.Make(template, format);
		}


		// DateOnly


		public static string Make(
			this DateOnly value,
			string template,
			string format,
			DateOnly nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}
		
		
		public static string Make(
			this DateOnly value,
			string template,
			string format = null)
		{
			return value.Make(template, format, DateOnly.MinValue);
		}
		
		
		public static string Make(
			this DateOnly? value,
			string template,
			string format = null)
		{
			return value?.Make(template, format);
		}


		// TimeOnly


		public static string Make(
			this TimeOnly value,
			string template,
			string format,
			TimeOnly nullValue)
		{
			return value == nullValue
				? null
				: (string.IsNullOrEmpty(format)
					? value.ToString()
					: value.ToString(format))
						.Make(template);
		}
		
		
		public static string Make(
			this TimeOnly value,
			string template,
			string format = null)
		{
			return value.Make(template, format, TimeOnly.MinValue);
		}
		
		
		public static string Make(
			this TimeOnly? value,
			string template,
			string format = null)
		{
			return value?.Make(template, format);
		}


		// bool


		public static string Make(
			this bool value,
			string trueText,
			string falseText = null)
		{
			return value
				? trueText : falseText;
		}


		public static string MakeMore(
			this bool value,
			string template,
			params object[] args)
		{
			return value
				? string.Format(template, args) : null;
		}


		// others


		public static string MakeRepeats(
			this string value,
			int count,
			string resultTemplate = null,
			string itemTemplate = null,
			string itemsSeparator = null)
		{
			if (string.IsNullOrEmpty(value) || count < 1)
				return null;
			var s1 = value.Make(itemTemplate);
			var a1 = Enumerable.Repeat(s1, count);
			return string.Join(itemsSeparator, a1)
				.Make(resultTemplate);
		}


		public static string MakeFromCollection(
			this IEnumerable<string> items,
			string resultTemplate,
			string itemTemplate,
			string itemsSeparator)
		{
			if (!(items?.Count() > 0))
				return null;
			Func<string, string> func1 = string.IsNullOrEmpty(itemTemplate)
				? x => x : x => x.Make(itemTemplate);
			var a1 = items.Where(x => !string.IsNullOrEmpty(x)).Select(func1);
			return string.Join(itemsSeparator, a1)
				.Make(resultTemplate);
		}


		public static string MakeFromCollection<T>(
			this IEnumerable<T> items,
			Func<T, string> itemExtractor,
			string resultTemplate,
			string itemTemplate,
			string itemsSeparator)
		{
			return MakeFromCollection(
				items?.Select(itemExtractor), resultTemplate, itemTemplate, itemsSeparator);
		}


		public static string MakeOverDict<T>(
			this string key,
			Dictionary<string, T> dictionary,
			Func<T, string> func,
			string template)
		{
			if (string.IsNullOrEmpty(key))
				return null;
			return dictionary
				.GetValueStringOrKey(key, func)
				.Make(template);
		}
		
		
		public static string MakeOverDict(
			this string key,
			Dictionary<string, string> dictionary,
			string template)
		{
			return MakeOverDict(
				key, dictionary, x => x, template);
		}


		public static string Make_UrlWrapper(
			this string url,
			string template)
		{
			if (string.IsNullOrEmpty(url))
				return null;
			if (url[0] == '/'
				|| url.StartsWith("https://")
				|| url.StartsWith("http://"))
				return url.Make(template);
			return string.Concat("https://", url).Make(template);
		}


		public static string Make_Passed(
			this DateTime datetime,
			DateTimeHelper calc,
			bool addTime,
			bool useYesterdayTodayTomorrow)
		{
			return calc.GetPassed(
				datetime, addTime, useYesterdayTodayTomorrow);
		}
		
		
		public static string Make_Passed(
			this DateTime? datetime,
			DateTimeHelper calc,
			bool addTime,
			bool useYesterdayTodayTomorrow)
		{
			return calc.GetPassed(
				datetime, addTime, useYesterdayTodayTomorrow);
		}
		
		
		public static string Make_Passed(
			this DateOnly date,
			DateTimeHelper calc,
			bool useYesterdayTodayTomorrow)
		{
			return calc.GetPassed(
				date, false, useYesterdayTodayTomorrow);
		}
		
		
		public static string Make_Passed(
			this DateOnly? date,
			DateTimeHelper calc,
			bool useYesterdayTodayTomorrow)
		{
			return calc.GetPassed(
				date, false, useYesterdayTodayTomorrow);
		}


		public static string Make_Span(
			this DateTime datetime1,
			DateTime? datetime2,
			bool showCurrentYear)
		{
			return SuppDateTime.GetSpan(
				datetime1, datetime2, showCurrentYear);
		}
		
		
		public static string Make_Span(
			this DateOnly date1,
			DateOnly? date2,
			bool showCurrentYear)
		{
			return SuppDateTime.GetSpan(
				date1, date2, showCurrentYear);
		}


		public static string Make_UniDate(
			this DateTime value)
		{
			return value.ToString("yyyy-MM-dd");
		}


		public static string Make_AnsDate(
			this DateTime value)
		{
			return value.ToString("yyyy-0MM-dd");
		}


		public static string Make_SizeOfKB(
			this long size)
		{
			return SuppIO.GetLengthOfKB(size);
		}
		
		
		public static string Make_SizeOfKB(
			this int size)
		{
			return SuppIO.GetLengthOfKB(size);
		}

	}

}
