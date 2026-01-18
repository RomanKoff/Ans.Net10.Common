namespace Ans.Net10.Common
{

	public static partial class __e_to
	{

		/* functions */


		public static IEnumerable<IEnumerable<T>> ToGrid<T>(
			this IEnumerable<T> source,
			int count)
		{
			return source
				.Select((x, y) => new { Index = y, Value = x })
				.GroupBy(x => x.Index / count)
				.Select(x => x.Select(y => y.Value));
		}


		// int


		public static int[] ToIntArray(
			this string[] source)
		{
			return [.. source.Select(x => x.ToInt(0))];
		}


		public static int[] ToIntArray(
			this string source)
		{
			return (string.IsNullOrEmpty(source))
				? null
				: source.Split(_Consts.SEPS_ARRAY).ToIntArray();
		}


		public static int ToIntRound(
			this double value)
		{
			return (int)(value + .5d);
		}


		public static int ToIntRound(
			this float value)
		{
			return (int)(value + .5f);
		}


		public static int ToIntRound(
			this decimal value)
		{
			return (int)(value + .5m);
		}


		public static int? ToInt(
			this string value)
		{
			if (int.TryParse(value, out int v1))
				return v1;
			return null;
		}


		public static int ToInt(
			this string value,
			int defaultValue)
		{
			return value.ToInt() ?? defaultValue;
		}


		// uint


		public static uint? ToUInt(
			this string value)
		{
			if (uint.TryParse(value, out uint v1))
				return v1;
			return null;
		}


		public static uint ToUInt(
			this string value,
			uint defaultValue)
		{
			return value.ToUInt() ?? defaultValue;
		}


		// long


		public static long? ToLong(
			this string value)
		{
			if (long.TryParse(value, out long v1))
				return v1;
			return null;
		}


		public static long ToLong(
			this string value,
			long defaultValue)
		{
			return value.ToLong() ?? defaultValue;
		}


		// double


		public static double? ToDouble(
			this string value)
		{
			if (double.TryParse(value, out double v1))
				return v1;
			return null;
		}


		public static double ToDouble(
			this string value,
			double defaultValue)
		{
			return value.ToDouble() ?? defaultValue;
		}


		// float


		public static float? ToFloat(
			this string value)
		{
			if (float.TryParse(value, out float v1))
				return v1;
			return null;
		}


		public static float ToFloat(
			this string value,
			float defaultValue)
		{
			return value.ToFloat() ?? defaultValue;
		}


		// decimal


		public static decimal? ToDecimal(
			this string value)
		{
			if (decimal.TryParse(value, out decimal v1))
				return v1;
			return null;
		}


		public static decimal ToDecimal(
			this string value,
			decimal defaultValue)
		{
			return value.ToDecimal() ?? defaultValue;
		}


		// bool


		public static bool ToBool(
			this string value)
		{
			if (value == null || value.Length > 4)
				return false;
			if (value.Length < 4)
				return value == "1";
			return value == "true"
				|| value == "True"
				|| value == "TRUE";
		}


		// DateTime


		public static DateTime? ToDateTime(
			 this string value)
		{
			if (DateTime.TryParse(value, out DateTime v1))
				return v1;
			return null;
		}


		public static DateTime ToDateTime(
			this string value,
			DateTime defaultValue)
		{
			return value.ToDateTime() ?? defaultValue;
		}


		// DateOnly


		public static DateOnly? ToDateOnly(
			this string value)
		{
			if (DateOnly.TryParse(value, out DateOnly v1))
				return v1;
			return null;
		}


		public static DateOnly ToDateOnly(
			this string value,
			DateOnly defaultValue)
		{
			return value.ToDateOnly() ?? defaultValue;
		}


		// TimeOnly


		public static TimeOnly? ToTimeOnly(
			this string value)
		{
			if (TimeOnly.TryParse(value, out TimeOnly v1))
				return v1;
			return null;
		}


		public static TimeOnly ToTimeOnly(
			this string value,
			TimeOnly defaultValue)
		{
			return value.ToTimeOnly() ?? defaultValue;
		}

	}

}
