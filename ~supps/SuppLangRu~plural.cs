// rev 2026-09-16

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class SuppLangRu
	{

		/// <summary>
		/// Возвращает грамматически корректную форму русского существительного в зависимости от числительного.
		/// </summary>
		/// <param name="value">Количество объектов (поддерживает большие числа long).</param>
		/// <param name="form1">Форма для числительных, оканчивающихся на 1, кроме 11 (например, "день", "яблоко").</param>
		/// <param name="form2to4">Форма для числительных, оканчивающихся на 2-4, кроме 12-14 (например, "дня", "яблока").</param>
		/// <param name="form0and5to9and11to14">Форма для числительных, оканчивающихся на 0, 5-9, а также от 11 до 14 (например, "дней", "яблок").</param>
		/// <returns>Строка с существительным в правильной грамматической форме.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetPlural(
			long value,
			string form1,
			string form2to4,
			string form0and5to9and11to14)
		{
			long absCount1 = Math.Abs(value);
			long remainder100 = absCount1 % 100;
			long remainder10 = absCount1 % 10;
			return remainder100 switch
			{
				>= 11 and <= 14 => form0and5to9and11to14,
				_ => remainder10 switch
				{
					1 => form1,
					>= 2 and <= 4 => form2to4,
					_ => form0and5to9and11to14
				}
			};
		}


		/// <summary>
		/// Возвращает грамматически корректную форму русского существительного в зависимости от числа, подставленную в указанный шаблон.
		/// </summary>
		/// <param name="template">Шаблон форматирования (например, "Осталось {0} {1}"). Параметр {0} — число, {1} — слово.</param>
		/// <param name="value">Количество объектов.</param>
		/// <param name="form1">Форма для 1 (например, "день").</param>
		/// <param name="form2to4">Форма для 2-4 (например, "дня").</param>
		/// <param name="form0and5to9and11to14">Форма для 0, 5-9, 11-14 (например, "дней").</param>
		/// <returns>Форматированная строка, либо <see cref="string.Empty"/>, если шаблон пуст.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetPlural(
			string? template,
			long value,
			string form1,
			string form2to4,
			string form0and5to9and11to14)
		{
			if (string.IsNullOrWhiteSpace(template))
				return string.Empty;
			var s1 = GetPlural(value, form1, form2to4, form0and5to9and11to14);
			return string.Format(template, value, s1);
		}


		/// <summary>
		/// Возвращает грамматически корректную форму возраста (числа лет) в формате "N год/года/лет".
		/// </summary>
		/// <param name="year">Количество лет.</param>
		/// <returns>Форматированная строка с указанием возраста.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetPluralAge(
			int year)
		{
			return GetPlural("{0} {1}", year, "год", "года", "лет");
		}

	}

}
