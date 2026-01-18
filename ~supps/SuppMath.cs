namespace Ans.Net10.Common
{

	public static class SuppMath
	{

		/* functions */


		/// <summary>
		/// Округление до ближайшего целого (.4,.5)
		/// </summary>
		public static int RoundToInt(
			double value)
		{
			return (int)Math.Round(
				value, MidpointRounding.AwayFromZero);
		}


		/// <summary>
		/// Округление до ближайшего целого (.4,.5)
		/// </summary>
		public static int RoundToInt(
			decimal value)
		{
			return (int)Math.Round(
				value, MidpointRounding.AwayFromZero);
		}


		/// <summary>
		/// Округление до ближайшего целого (.4,.5)
		/// </summary>
		public static uint RoundToUInt(
			double value)
		{
			return (uint)Math.Round(
				value, MidpointRounding.AwayFromZero);
		}


		/// <summary>
		/// Округление до ближайшего целого (.4,.5)
		/// </summary>
		public static uint RoundToUInt(
			decimal value)
		{
			return (uint)Math.Round(
				value, MidpointRounding.AwayFromZero);
		}


		/// <summary>
		/// Масштабирование по подобию
		/// </summary>
		public static double Map(
			double value,
			double fromMin,
			double fromMax,
			double toMin,
			double toMax,
			bool crop)
		{
			if (crop && value < fromMin)
				return toMin;
			if (crop && value > fromMax)
				return toMax;
			double normal1 = (value - fromMin) / (fromMax - fromMin);
			double abs1 = (toMax - toMin) * normal1;
			return abs1 + toMin;
		}


		/// <summary>
		/// Масштабирование по подобию
		/// </summary>
		public static int Map(
			int value,
			int fromMin,
			int fromMax,
			int toMin,
			int toMax,
			bool crop = true)
		{
			return RoundToInt(Map(
				(double)value,
				fromMin,
				fromMax,
				toMin,
				toMax,
				crop));
		}


		public static int GetNextDivisible(
			int value,
			int div = 8)
		{
			int v1 = value, t1;
			if ((t1 = value % div) != 0)
				v1 += (value > -1) ? (div - t1) : -t1;
			return v1;
		}


		public static float GetNextDivisible(
			float value,
			int div = 8)
		{
			float v1 = value, t1;
			if ((t1 = value % div) != 0)
				v1 += (value > -1) ? (div - t1) : -t1;
			return v1;
		}


		public static int GetRestrict(
			int value,
			int minLimit,
			int maxLimit)
		{
			return (value < minLimit)
				? minLimit
				: (value > maxLimit)
					? maxLimit
					: value;
		}


		public static long GetRestrict(
			long value,
			long minLimit,
			long maxLimit)
		{
			return (value < minLimit)
				? minLimit
				: (value > maxLimit)
					? maxLimit
					: value;
		}


		public static double GetRestrict(
			double value,
			double minLimit,
			double maxLimit)
		{
			return (value < minLimit)
				? minLimit
				: (value > maxLimit)
					? maxLimit
					: value;
		}


		public static float GetRestrict(
			float value,
			float minLimit,
			float maxLimit)
		{
			return (value < minLimit)
				? minLimit
				: (value > maxLimit)
					? maxLimit
					: value;
		}


		public static decimal GetRestrict(
			decimal value,
			decimal minLimit,
			decimal maxLimit)
		{
			return (value < minLimit)
				? minLimit
				: (value > maxLimit)
					? maxLimit
					: value;
		}


		public static DateTime GetRestrict(
			DateTime value,
			DateTime minLimit,
			DateTime maxLimit)
		{
			return (value < minLimit)
				? minLimit
				: (value > maxLimit)
					? maxLimit
					: value;
		}


		/// <summary>
		/// Возвращает индекс ближайшей точки, после которой находится value
		/// </summary>
		public static int GetRangeIndex(
			int value,
			params int[] points)
		{
			for (var i1 = 0; i1 < points.GetUpperBound(0); i1++)
				if (value >= points[i1] && value < points[i1 + 1])
					return i1;
			return -1;
		}

	}

}
