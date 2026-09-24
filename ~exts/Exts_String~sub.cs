// rev 2026-09-14

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class Exts_String
	{

		/// <summary>
		/// Возвращает левую часть строки до указанного символа, пропуская заданное количество вхождений с конца строки.
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="find">Искомый символ границы.</param>
		/// <param name="skip">Количество вхождений символа с конца строки, которые нужно пропустить.</param>
		/// <returns>Левая часть строки до найденного символа или пустая строка.</returns>
		public static string GetLeftTo(
			this string? instance,
			char find,
			int skip = 0)
		{
			if (string.IsNullOrEmpty(instance) || skip < 0)
				return string.Empty;
			int currentMatch1 = 0;
			for (int i1 = instance.Length - 1; i1 >= 0; i1--)
				if (instance[i1] == find)
				{
					if (currentMatch1 == skip)
						return instance[..i1];
					currentMatch1++;
				}
			return string.Empty;
		}


		/// <summary>
		/// Возвращает левую часть строки до указанной подстроки, пропуская заданное количество вхождений с конца строки.
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="find">Искомая подстрока границы.</param>
		/// <param name="skip">Количество вхождений подстроки с конца строки, которые нужно пропустить.</param>
		/// <returns>Левая часть строки до найденной подстроки или пустая строка.</returns>
		public static string GetLeftTo(
			this string? instance,
			string? find,
			int skip = 0)
		{
			if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(find) || skip < 0)
				return string.Empty;
			int currentMatch1 = 0;
			int searchIndex1 = instance.Length - 1;
			while (searchIndex1 >= 0)
			{
				int matchIndex1 = instance.LastIndexOf(find, searchIndex1, StringComparison.Ordinal);
				if (matchIndex1 == -1)
					return string.Empty;
				if (currentMatch1 == skip)
					return instance[..matchIndex1];
				currentMatch1++;
				searchIndex1 = matchIndex1 - 1;
			}
			return string.Empty;
		}


		/// <summary>
		/// Возвращает левую часть строки из первых <paramref name="count"/> символов.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetLeft(
			this string? instance,
			int count)
		{
			if (string.IsNullOrEmpty(instance) || count <= 0)
				return string.Empty;
			return count >= instance.Length ? instance : instance[..count];
		}


		/// <summary>
		/// Возвращает правую часть строки после указанного символа, пропуская заданное количество вхождений с конца строки.
		/// </summary>
		public static string GetRightFrom(
			this string? instance,
			char find,
			int skip = 0)
		{
			if (string.IsNullOrEmpty(instance) || skip < 0)
				return string.Empty;
			int currentMatch1 = 0;
			for (int i1 = instance.Length - 1; i1 >= 0; i1--)
				if (instance[i1] == find)
				{
					if (currentMatch1 == skip)
						return instance[(i1 + 1)..];
					currentMatch1++;
				}
			return string.Empty;
		}


		/// <summary>
		/// Возвращает правую часть строки после указанной подстроки, пропуская заданное количество вхождений с конца строки.
		/// </summary>
		public static string GetRightFrom(
			this string? instance,
			string? find,
			int skip = 0)
		{
			if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(find) || skip < 0)
				return string.Empty;
			int currentMatch1 = 0;
			int searchIndex1 = instance.Length - 1;
			while (searchIndex1 >= 0)
			{
				int matchIndex1 = instance.LastIndexOf(find, searchIndex1, StringComparison.Ordinal);
				if (matchIndex1 == -1)
					return string.Empty;
				if (currentMatch1 == skip)
					return instance[(matchIndex1 + find.Length)..];
				currentMatch1++;
				searchIndex1 = matchIndex1 - 1;
			}
			return string.Empty;
		}


		/// <summary>
		/// Возвращает правую часть строки из последних <paramref name="count"/> символов.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetRight(
			this string? instance,
			int count)
		{
			if (string.IsNullOrEmpty(instance) || count <= 0)
				return string.Empty;
			return count >= instance.Length ? instance : instance[^count..];
		}


		/// <summary>
		/// Возвращает правую часть строки, отрезая все, что находится до ПЕРВОГО вхождения символа слева (включая сам символ).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetRightSide(
			this string? instance,
			char find)
		{
			if (string.IsNullOrEmpty(instance))
				return string.Empty;
			int i1 = instance.IndexOf(find);
			return i1 == -1 ? string.Empty : instance[(i1 + 1)..];
		}


		/// <summary>
		/// Возвращает правую часть строки, отрезая все, что находится до ПЕРВОГО вхождения подстроки слева (включая саму подстроку).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetRightSide(
			this string? instance,
			string find)
		{
			if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(find))
				return string.Empty;
			int i1 = instance.IndexOf(find);
			return i1 == -1 ? string.Empty : instance[(i1 + find.Length)..];
		}


		/// <summary>
		/// Возвращает правую часть строки БЕЗ указанного количества символов слева.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetRightSide(
			this string? instance,
			int count)
		{
			if (string.IsNullOrEmpty(instance) || count <= 0)
				return string.Empty;
			return instance.Length <= count ? string.Empty : instance[count..];
		}


		/// <summary>
		/// Возвращает левую часть строки, отрезая все, что находится после ПЕРВОГО вхождения символа справа (включая сам символ).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetLeftSide(
			this string? instance,
			char find)
		{
			if (string.IsNullOrEmpty(instance))
				return string.Empty;
			int i1 = instance.LastIndexOf(find);
			return i1 == -1 ? string.Empty : instance[..i1];
		}


		/// <summary>
		/// Возвращает левую часть строки, отрезая все, что находится после ПЕРВОГО вхождения подстроки справа (включая саму подстроку).
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetLeftSide(
			this string? instance,
			string find)
		{
			if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(find))
				return string.Empty;
			int i1 = instance.LastIndexOf(find);
			return i1 == -1 ? string.Empty : instance[..i1];
		}


		/// <summary>
		/// Возвращает левую часть строки БЕЗ указанного количества символов справа.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetLeftSide(
			this string? instance,
			int count)
		{
			if (string.IsNullOrEmpty(instance) || count <= 0)
				return string.Empty;
			return instance.Length <= count ? string.Empty : instance[0..^count];
		}


		/// <summary>
		/// Сокращает текст до указанной максимальной длины, не разрывая слова на полуслове. 
		/// Если текст обрезан, в конец добавляется многоточие.
		/// </summary>
		/// <param name="instance">Исходный текст.</param>
		/// <param name="maxLength">Максимально допустимая длина результирующей строки.</param>
		/// <param name="ellipsis">Строка-суффикс, добавляемая при обрезке.</param>
		public static string GetCropToWords(
			this string? instance,
			int maxLength,
			string ellipsis = "…")
		{
			if (string.IsNullOrEmpty(instance) || maxLength <= 0)
				return string.Empty;
			if (instance.Length <= maxLength)
				return instance;
			int ellipsisLength1 = ellipsis?.Length ?? 0;
			int maxTextLength1 = maxLength - ellipsisLength1;
			if (maxTextLength1 <= 0)
				return instance[..maxLength];
			var span1 = instance.AsSpan();
			int cutIndex1 = maxTextLength1;
			if (char.IsWhiteSpace(span1[cutIndex1]) || char.IsPunctuation(span1[cutIndex1]))
				return string.Concat(span1[..cutIndex1].TrimEnd(), ellipsis);
			while (cutIndex1 > 0)
			{
				char ch1 = span1[cutIndex1 - 1];
				if (char.IsWhiteSpace(ch1) || char.IsPunctuation(ch1))
					break;
				cutIndex1--;
			}
			if (cutIndex1 == 0)
				cutIndex1 = maxTextLength1;
			return string.Concat(span1[..cutIndex1].TrimEnd(), ellipsis);
		}


		/// <summary>
		/// Возвращает подстроку с подстановкой маски по обрезанным краям.
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="startIndex">Начальный индекс подстроки.</param>
		/// <param name="length">Длина извлекаемого фрагмента.</param>
		/// <param name="beginCropMask">Маска, подставляемая в начало, если левый край был обрезан.</param>
		/// <param name="endCropMask">Маска, подставляемая в конец, если правый край был обрезан.</param>
		public static string GetCrop(
			this string? instance,
			int startIndex,
			int length,
			string? beginCropMask = null,
			string? endCropMask = null)
		{
			if (string.IsNullOrEmpty(instance) || length <= 0 || startIndex >= instance.Length)
				return string.Empty;
			if (startIndex < 0)
				startIndex = 0;
			int len1 = instance.Length;
			bool hasBeginCrop1 = startIndex > 0;
			int len2 = len1 - startIndex;
			bool hasEndCrop1 = len2 > length;
			int len3 = hasEndCrop1 ? length : len2;
			var s1 = instance.AsSpan(startIndex, len3);
			string maskStart1 = beginCropMask ?? "…";
			string maskEnd1 = endCropMask ?? beginCropMask ?? "…";
			return string.Concat(
				hasBeginCrop1 ? maskStart1 : string.Empty,
				s1,
				hasEndCrop1 ? maskEnd1 : string.Empty);
		}

	}

}
