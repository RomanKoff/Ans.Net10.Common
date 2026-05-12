namespace Ans.Net10.Common
{

	public enum RegistryModeEnum
	{
		Auto,
		Inputs,
		Select
	}



	public enum WidthEnum
	{
		Nothing,
		ExtraSmall,
		Small,
		Medium,
		Large,
		ExtraLarge,
		Full,
	}



	public enum EncodingsEnum
	{
		UTF8,
		WINDOWS1251,
		KOI8R,
		CP866,
		ISO88591,
	}



	public enum LetterCasesEnum
	{
		/// <summary>
		/// Без изменений
		/// </summary>
		Original,

		/// <summary>
		/// Нижний регистр (строчные)
		/// </summary>
		Lower,

		/// <summary>
		/// Верхний регистр (ПРОПИСНЫЕ, ЗАГЛАВНЫЕ)
		/// </summary>
		Upper,

		/// <summary>
		/// Первая буква строки заглавная
		/// </summary>
		FirstUpper,

		/// <summary>
		/// Заглавная Первая Буква В Каждом Слове Строки
		/// </summary>
		StartWithACapital
	}



	public enum WordCasesEnum
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



	public enum GenderEnum
		: int
	{
		/// <summary>
		/// Не указан
		/// </summary>
		NotSpecified = 0,

		/// <summary>
		/// Мужской
		/// </summary>
		Male = 1,

		/// <summary>
		/// Женский
		/// </summary>
		Female = 2,
	}

}
