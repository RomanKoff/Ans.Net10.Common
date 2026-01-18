using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Ans.Net10.Common
{

	public static partial class _e_String
	{

		public static string[] SplitFix(
			this string source,
			string separator,
			int count)
		{
			var a1 = source.Split(separator);
			if (a1.Length >= count)
				return [.. a1.Take(count)];
			var a2 = new string[count];
			foreach (var (item1, index1) in a1.WithIndex())
				a2[index1] = item1;
			return a2;
		}


		public static string ReplaceFromDict(
			this string source,
			Dictionary<string, string> dict)
		{
			var sb1 = new StringBuilder(source);
			foreach (var item1 in dict)
				sb1.Replace(item1.Key, item1.Value);
			return sb1.ToString();

			/*
			
			:start
			ch1
			:next
			items.Where(x=>x.StartWith(ch1))
			items.Count
				0
					add(ch1); start;
				1
					replace(ch1, item.value); start;
				*
					ch1 += ch2; next;

			 */

		}

		
		public static string ReplaceIfEqual(
			this string source,
			string compared,
			string newest)
		{
			return (source == compared)
				? newest : source;
		}


		public static string ReplaceByChars(
			this string source,
			string mask,
			string charsPattern)
		{
			var r1 = new Regex($"[{charsPattern}]");
			return r1.Replace(source, mask);
		}


		public static string ReplaceByChars(
			this string source,
			string mask,
			params char[] chars)
		{
			return source.ReplaceByChars(mask, string.Join("", chars));
		}


		public static string ReplaceKeysFromDict(
			this string source,
			Dictionary<string, string> dictionary,
			char marker = '#')
		{
			var sb1 = new StringBuilder();
			for (int i1 = 0; i1 < source.Length; i1++)
			{
				var c1 = source[i1];
				if (c1 != marker)
				{
					sb1.Append(c1);
					continue;
				}
				int i2 = i1 + 1;
				int l1 = source.Length - i2 + 1;
				bool notFound1 = true;
				foreach (var item1 in dictionary)
					if (item1.Key.Length < l1)
					{
						bool isMatch1 = true;
						for (int i3 = 0; i3 < item1.Key.Length; i3++)
						{
							if (item1.Key[i3] != source[i2 + i3])
							{
								isMatch1 = false;
								break;
							}
						}
						if (isMatch1)
						{
							notFound1 = false;
							sb1.Append(item1.Value);
							i1 += item1.Key.Length;
							break;
						}
					}
				if (notFound1)
					sb1.Append(c1);
			}
			return sb1.ToString();
		}


		public static string GetTrimEnd(
			this string source,
			string trimString)
		{
			var s1 = SuppRegex.ParamEncode(trimString);
			return Regex.Replace(source, $"(?:{s1})+", string.Empty);
		}


		public static string GetTrimStart(
			this string source,
			string trimString)
		{
			var s1 = SuppRegex.ParamEncode(trimString);
			return Regex.Replace(source, $"({s1}:?)+", string.Empty);
		}


		public static string GetFirstUpper(
			this string source,
			bool forcedToLower)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int l1 = source.Length;
			if (l1 == 1)
				return source.ToUpper();
			string s1 = (forcedToLower)
				? source[1..].ToLower()
				: source[1..];
			return $"{char.ToUpper(source[0])}{s1}";
		}


		public static string GetFirstLower(
			this string source,
			bool forcedToUpper)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int l1 = source.Length;
			if (l1 == 1)
				return source.ToLower();
			string s1 = (forcedToUpper)
				? source[1..].ToUpper()
				: source[1..];
			return $"{char.ToLower(source[0])}{s1}";
		}


		public static string GetStartWithACapital(
			this string source,
			CultureInfo cultureInfo)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			return cultureInfo.TextInfo.ToTitleCase(source);
		}


		public static string GetStartWithACapital(
			this string source)
		{
			return source.GetStartWithACapital(
				CultureInfo.CurrentCulture);
		}


		/// <summary>
		/// Возвращает левую часть сторки до символа 'find'
		/// </summary>
		public static string GetLeft(
			this string source,
			char find)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int i1 = source.IndexOf(find);
			if (i1 == -1)
				return string.Empty;
			return source[..i1];
		}


		/// <summary>
		/// Возвращает левую часть сторки до подстроки 'find'
		/// </summary>
		public static string GetLeft(
			this string source,
			string find)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int i1 = source.IndexOf(find);
			if (i1 == -1)
				return string.Empty;
			return source[..i1];
		}


		/// <summary>
		/// Возвращает левую часть сторки из 'count' символов
		/// </summary>
		public static string GetLeft(
			this string source,
			int count)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int l1 = source.Length;
			if (l1 <= count)
				return source;
			return source[..count];
		}


		/// <summary>
		/// Возвращает правую часть сторки до символа 'find'
		/// </summary>
		public static string GetRight(
			this string source,
			char find)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int i1 = source.LastIndexOf(find);
			if (i1 == -1)
				return string.Empty;
			return source[(i1 + 1)..];
		}


		/// <summary>
		/// Возвращает правую часть сторки до подстроки 'find'
		/// </summary>
		public static string GetRight(
			this string source,
			string find)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int i1 = source.LastIndexOf(find);
			if (i1 == -1)
				return string.Empty;
			return source[(i1 + find.Length)..];
		}


		/// <summary>
		/// Возвращает правую часть сторки из 'count' символов
		/// </summary>
		public static string GetRight(
			this string source,
			int count)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int l1 = source.Length;
			if (l1 <= count)
				return source;
			return source[(l1 - count)..];
		}


		/// <summary>
		/// Возвращает левую часть сторки до символа 'find'
		/// </summary>
		public static string GetBackTo(
			this string source,
			char find,
			int skip = 0)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int step1 = 0;
			int i1 = source.Length - 1;
			int i2;
			do
			{
				i2 = source.LastIndexOf(find, i1);
				i1 = i2 - 1;
				step1++;
			} while (step1 <= skip && i2 != -1);
			if (i2 == -1)
				return string.Empty;
			return source[..i2];
		}


		/// <summary>
		/// Возвращает подстроку с подстановкой по обрезанным краям
		/// </summary>
		public static string GetCrop(
			this string source,
			int startIndex,
			int length,
			string beginCropMask = null,
			string endCropMask = null)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int l1 = source.Length;
			if (source == null || l1 == 0 || startIndex >= l1)
				return string.Empty;
			int i1 = l1 - startIndex;
			bool f1 = startIndex > 0;
			bool f2 = false;
			if (i1 > length)
			{
				f2 = true;
				i1 = length;
			}
			string s1 = source[startIndex..(startIndex + i1)];
			return string.Concat(f1.Make(beginCropMask ??= "…"), s1, f2.Make(endCropMask ?? beginCropMask));
		}


		/// <summary>
		/// Возвращает результат преобразования символов
		/// с кодом меньше 33 в '_' и больше 126 в их кодовый эквивалент
		/// </summary>
		public static string GetSafeText(
			this string source)
		{
			var sb1 = new StringBuilder(source);
			foreach (char ch1 in source)
			{
				int code1 = ch1;
				sb1.Append(
					(code1 < 33) ? '_' : (code1 > 126) ? $"~{code1}~" : ch1);
			}
			return sb1.ToString();
		}


		/// <summary>
		/// Возвращает из строки подстроку находящуюся между 'before' и 'after'
		/// </summary>
		public static string GetTag(
			this string source,
			string before,
			string after,
			StringComparison comparisonType = StringComparison.InvariantCulture)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			int i1 = (string.IsNullOrEmpty(before))
				? 0 : source.IndexOf(before, comparisonType);
			if (i1 < 0)
				return string.Empty;
			i1 += before.Length;
			int i2 = (string.IsNullOrEmpty(after))
				? source.Length
				: source.IndexOf(after, i1, comparisonType);
			if (i2 < 0)
				return string.Empty;
			return source[i1..i2];
		}


		/// <summary>
		/// Производит рекурсивную замену 'oldValue' на 'newValue'
		/// </summary>
		public static string GetReplaceRecursively(
			this string source,
			string oldValue,
			string newValue)
		{
			int i1 = source.IndexOf(oldValue, 0);
			if (i1 < 0)
				return source;
			return source
				.Replace(oldValue, newValue)
				.GetReplaceRecursively(oldValue, newValue);
		}

	}

}
