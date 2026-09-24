// rev 2026-09-16

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public enum WordCasesRuEnum
	{
		/// <summary>
		/// Именительный падеж (кто, что)
		/// </summary>
		Nominative,

		/// <summary>
		/// Родительный падеж (кого, чего)
		/// </summary>
		Genitive,

		/// <summary>
		/// Дательный падеж (кому, чему)
		/// </summary>
		Dative,

		/// <summary>
		/// Винительный падеж (кого, что)
		/// </summary>
		Accusative,

		/// <summary>
		/// Творительный падеж (кем, чем)
		/// </summary>
		Instrumental,

		/// <summary>
		/// Предложный падеж (о ком, о чём)
		/// </summary>
		Prepositional
	}



	/// <summary>
	/// Вспомогательный класс для работы с русским языком.
	/// </summary>
	public static partial class SuppLangRu
	{

		/// <summary>
		/// Возвращает вычисленный биологический пол человека на основе
		/// анализа окончания его фамилии, имени или отчества.
		/// </summary>
		/// <param name="fullname">Строка, содержащая ФИО, фамилию с инициалами или только отчество.</param>
		/// <returns>
		/// Определенный <see cref="GenderEnum"/> или <see cref="GenderEnum.NotSpecified"/>,
		/// если пол определить не удалось.
		/// </returns>
		/// <example>
		/// <code>
		/// SuppLangRu.GetGender("Пушкин Александр Сергеевич"); // Male
		/// SuppLangRu.GetGender("Эфендиев Эльчин Ильяс оглы"); // Male
		/// SuppLangRu.GetGender("Ахмедова Лейла кызы"); // Female
		/// </code>
		/// </example>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GenderEnum GetGender(
			string fullname)
		{
			if (string.IsNullOrWhiteSpace(fullname))
				return GenderEnum.NotSpecified;
			var span1 = fullname.AsSpan().Trim();
			if (span1.EndsWith("ич", StringComparison.OrdinalIgnoreCase))
				return GenderEnum.Male;
			if (span1.EndsWith("на", StringComparison.OrdinalIgnoreCase))
				return GenderEnum.Female;
			if (span1.EndsWith("глы", StringComparison.OrdinalIgnoreCase))
				return GenderEnum.Male;
			if (span1.EndsWith("ызы", StringComparison.OrdinalIgnoreCase))
				return GenderEnum.Female;
			return GenderEnum.NotSpecified;
		}

	}

}
