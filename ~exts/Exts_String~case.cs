// rev 2026-09-14

using System.Globalization;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class Exts_String
	{

		/// <summary>
		/// Преобразует регистр строки, делая первую букву заглавной.
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="forcedToLower">Принудительно сделать все остальные буквы строчными.</param>
		/// <param name="cultureInfo">Информация о культуре. Если не указана, используется <see cref="CultureInfo.InvariantCulture"/>.</param>
		/// <returns>Строка с первой заглавной буквой.</returns>
		public static string GetAsFirstUpper(
			this string? instance,
			bool forcedToLower = false,
			CultureInfo? cultureInfo = null)
		{
			if (string.IsNullOrEmpty(instance))
				return string.Empty;
			var cultureInfo1 = cultureInfo ?? CultureInfo.InvariantCulture;
			if (instance.Length == 1)
				return char.ToUpper(instance[0], cultureInfo1).ToString();
			return string.Create(
				instance.Length,
				(Src: instance, ForcedToLower: forcedToLower, Culture: cultureInfo1),
				(span1, state1) =>
				{
					var src1 = state1.Src;
					span1[0] = char.ToUpper(src1[0], state1.Culture);
					if (state1.ForcedToLower)
						src1.AsSpan(1).ToLower(span1[1..], state1.Culture);
					else
						src1.AsSpan(1).CopyTo(span1[1..]);
				});
		}


		/// <summary>
		/// Преобразует регистр строки, делая первую букву строчной.
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="forcedToUpper">Принудительно сделать все остальные буквы заглавными.</param>
		/// <param name="cultureInfo">Информация о культуре. Если не указана, используется <see cref="CultureInfo.InvariantCulture"/>.</param>
		/// <returns>Строка с первой строчной буквой.</returns>
		public static string GetAsFirstLower(
			this string? instance,
			bool forcedToUpper = false,
			CultureInfo? cultureInfo = null)
		{
			if (string.IsNullOrEmpty(instance))
				return string.Empty;
			var cultureInfo1 = cultureInfo ?? CultureInfo.InvariantCulture;
			if (instance.Length == 1)
				return char.ToLower(instance[0], cultureInfo1).ToString();
			return string.Create(
				instance.Length,
				(Src: instance, ForcedToUpper: forcedToUpper, Culture: cultureInfo1),
				(span1, state1) =>
				{
					var src1 = state1.Src;
					span1[0] = char.ToLower(src1[0], state1.Culture);
					if (state1.ForcedToUpper)
						src1.AsSpan(1).ToUpper(span1[1..], state1.Culture);
					else
						src1.AsSpan(1).CopyTo(span1[1..]);
				});
		}


		/// <summary>
		/// Преобразует регистр строки, начиная каждое слово с заглавной буквы (Title Case).
		/// </summary>
		/// <param name="instance">Исходная строка.</param>
		/// <param name="cultureInfo">Информация о культуре. Если не указана, используется <see cref="CultureInfo.InvariantCulture"/>.</param>
		/// <returns>Строка в регистре Title Case.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetAsTitleCase(
			this string? instance,
			bool forcedToLower = false,
			CultureInfo? cultureInfo = null)
		{
			if (string.IsNullOrEmpty(instance))
				return string.Empty;
			var cultureInfo1 = cultureInfo ?? CultureInfo.InvariantCulture;
			var s1 = forcedToLower
				? instance.ToLower(cultureInfo) : instance;
			return cultureInfo1.TextInfo.ToTitleCase(s1);
		}

	}

}
