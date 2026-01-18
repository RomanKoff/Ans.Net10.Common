using System.Text;

namespace Ans.Net10.Common
{

	public static partial class _e_StringBuilder
	{

		/* methods */


		public static void AppendIfPresent(
			this StringBuilder sb,
			string template,
			params object[] templateArgs)
		{
			if (!string.IsNullOrEmpty(template))
				sb.AppendFormat(template, templateArgs);
		}


		public static void InsertFormat(
			this StringBuilder sb,
			int index,
			string template,
			params object[] templateArgs)
		{
			sb.Insert(index, string.Format(template, templateArgs));
		}


		public static void InsertIfPresent(
			this StringBuilder sb,
			int index,
			string template,
			params object[] templateArgs)
		{
			if (!string.IsNullOrEmpty(template))
				sb.InsertFormat(index, template, templateArgs);
		}


		public static void InsertIf(
			this StringBuilder sb,
			bool expression,
			int index,
			string template,
			params object[] templateArgs)
		{
			if (expression)
				sb.InsertFormat(index, template, templateArgs);
		}


		/* functions */


		/// <summary>
		/// Возвращает индекс начала вхождения строки
		/// </summary>
		public static int IndexOf(
			this StringBuilder sb,
			string value,
			int startIndex,
			bool ignoreCase)
		{
			int length1 = value.Length;
			int max1 = (sb.Length - length1) + 1;
			var value1 = (ignoreCase) ? value.ToLower() : value;
			var func1 = (ignoreCase) ? _Consts.FuncToLower : _Consts.FuncSelf;
			for (int i1 = startIndex; i1 < max1; ++i1)
				if (func1(sb[i1]) == value1[0])
				{
					int i2 = 1;
					while ((i2 < length1) && (func1(sb[i1 + i2]) == value1[i2]))
						++i2;
					if (i2 == length1)
						return i1;
				}
			return -1;
		}


		public static StringBuilder ReplaceRecursively(
			this StringBuilder sb,
			string oldValue,
			string newValue,
			bool ignoreCase)
		{
			int i1 = sb.IndexOf(oldValue, 0, ignoreCase);
			if (i1 < 0)
				return sb;
			return sb
				.Replace(oldValue, newValue)
				.ReplaceRecursively(oldValue, newValue, ignoreCase);
		}


		public static void ReplaceSpecChars(
			this StringBuilder sb)
		{
			for (int i1 = 0; i1 < sb.Length; ++i1)
				if (sb[i1] < 32)
					sb[i1] = ' ';
		}


		public static void Trim(
			this StringBuilder sb)
		{
			if (sb.Length > 0)
			{
				int c1 = 0;
				for (var i1 = 0; i1 < sb.Length; i1++)
				{
					if (!char.IsWhiteSpace(sb[i1]))
						break;
					c1++;
				}
				if (c1 > 0)
				{
					sb.Remove(0, c1);
					c1 = 0;
				}
				for (var i1 = sb.Length - 1; i1 >= 0; i1--)
				{
					if (!char.IsWhiteSpace(sb[i1]))
						break;
					c1++;
				}
				if (c1 > 0)
					sb.Remove(sb.Length - c1, c1);
			}
		}


		public static void Trim(
			this StringBuilder sb,
			char[] chars)
		{
			if (sb.Length > 0)
			{
				int c1 = 0;
				for (var i1 = 0; i1 < sb.Length; i1++)
				{
					if (!chars.Contains(sb[i1]))
						break;
					c1++;
				}
				if (c1 > 0)
				{
					sb.Remove(0, c1);
					c1 = 0;
				}
				for (var i1 = sb.Length - 1; i1 >= 0; i1--)
				{
					if (!chars.Contains(sb[i1]))
						break;
					c1++;
				}
				if (c1 > 0)
					sb.Remove(sb.Length - c1, c1);
			}
		}

	}

}
