using System.Text;

namespace Ans.Net10.Common
{

	public enum TextCasesEnum
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



	public static class SuppLangRu
	{

		/* functions */


		public static string Sample()
			=> _Consts.GET_RANDOM_SAMPLE_RU();

		public static string SampleSmall()
			=> _Consts.GET_RANDOM_SAMPLE_SMALL_RU();

		public static string SampleSmaller()
			=> _Consts.GET_RANDOM_SAMPLE_SMALLER_RU();


		/// <summary>
		/// Возвращает транслитерацию char с русского на английский
		/// (https://www.ifap.ru/library/gost/70342014.pdf)
		/// </summary>
		public static string GetTranslitRuToEn(
			char value)
		{
			return value switch
			{
				'а' => "a",
				'б' => "b",
				'в' => "v",
				'г' => "g",
				'д' => "d",
				'е' => "e",
				'ё' => "yo",
				'ж' => "zh",
				'з' => "z",
				'и' => "i",
				'й' => "y",
				'к' => "k",
				'л' => "l",
				'м' => "m",
				'н' => "n",
				'о' => "o",
				'п' => "p",
				'р' => "r",
				'с' => "s",
				'т' => "t",
				'у' => "u",
				'ф' => "f",
				'х' => "kh",
				'ц' => "ts",
				'ч' => "ch",
				'ш' => "sh",
				'щ' => "shh",
				'ъ' => "",
				'ы' => "y",
				'ь' => "",
				'э' => "e",
				'ю' => "yu",
				'я' => "ya",
				_ => value.ToString(),
			};
		}


		/// <summary>
		/// Возвращает транслитерацию string с русского на английский
		/// (https://www.ifap.ru/library/gost/70342014.pdf)
		/// </summary>
		public static string GetTranslitRuToEn(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			var sb1 = new StringBuilder();
			foreach (var char1 in source)
				sb1.Append(GetTranslitRuToEn(char1));
			return sb1.ToString();
		}


		/// <summary>
		/// Возвращает транслитерацию StringBuilder с русского на английский
		/// (https://www.ifap.ru/library/gost/70342014.pdf)
		/// </summary>
		public static StringBuilder GetTranslitRuToEn(
			StringBuilder source)
		{
			var sb1 = new StringBuilder();
			foreach (var char1 in source.ToString())
				sb1.Append(GetTranslitRuToEn(char1));
			return sb1;
		}


		/// <summary>
		/// ё --> е
		/// </summary>
		public static char GetFixUmlautRu(
			char value)
		{
			return value switch
			{
				'ё' => 'е',
				'Ё' => 'Е',
				_ => value,
			};
		}


		/// <summary>
		/// ё --> е
		/// </summary>
		public static string GetFixUmlautRu(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			var sb1 = new StringBuilder();
			foreach (var char1 in source)
				sb1.Append(GetFixUmlautRu(char1));
			return sb1.ToString();
		}


		/// <summary>
		/// ё --> е
		/// </summary>
		public static StringBuilder GetFixUmlautRu(
			StringBuilder source)
		{
			var sb1 = new StringBuilder();
			foreach (var char1 in source.ToString())
				sb1.Append(GetFixUmlautRu(char1));
			return sb1;
		}


		/// <summary>
		/// № --> Nº
		/// </summary>
		public static string GetFixNumberRu(
			char value)
		{
			return value switch
			{
				'№' => "Nº",
				_ => value.ToString(),
			};
		}


		/// <summary>
		/// № --> Nº
		/// </summary>
		public static string GetFixNumberRu(
			string source)
		{
			if (string.IsNullOrEmpty(source))
				return string.Empty;
			var sb1 = new StringBuilder();
			foreach (var char1 in source)
				sb1.Append(GetFixNumberRu(char1));
			return sb1.ToString();
		}


		/// <summary>
		/// № --> Nº
		/// </summary>
		public static StringBuilder GetFixNumberRu(
			StringBuilder source)
		{
			var sb1 = new StringBuilder();
			foreach (var char1 in source.ToString())
				sb1.Append(GetFixNumberRu(char1));
			return sb1;
		}



		/// <summary>
		/// Возвращает склонение для числа
		/// </summary>
		public static string GetDeclineRu(
			int number,
			string for1,
			string forFrom2To4,
			string for0AndFrom5To9AndFrom11To19)
		{
			if (number > 10)
			{
				int r1 = number % 100;
				if (r1 > 10 && r1 < 20)
					return for0AndFrom5To9AndFrom11To19;
			}
			return (number % 10) switch
			{
				1 => for1,
				> 1 and < 5 => forFrom2To4,
				_ => for0AndFrom5To9AndFrom11To19
			};
		}


		/// <summary>
		/// Возвращает склонение для числа по шаблону 
		/// </summary>
		public static string GetDeclineRu(
			string template,
			int number,
			string for1,
			string forFrom2To4,
			string for0AndFrom5To9AndFrom11To19)
		{
			var s1 = GetDeclineRu(
				number,
				for1,
				forFrom2To4,
				for0AndFrom5To9AndFrom11To19);
			return string.Format(template, number, s1);
		}


		/// <summary>
		/// Возвращает склонение для числа лет
		/// </summary>
		public static string GetAgeStringRu(
			int year)
		{
			return GetDeclineRu("{0} {1}", year, "год", "года", "лет");
		}


		/// <summary>
		/// Возвращает значение целого числа прописью
		/// </summary>
		public static string GetWordsRu(
			long value,
			TextCasesEnum textCase = TextCasesEnum.Nominative,
			bool isMale = true,
			bool firstCapital = false)
		{
			long value1 = value;
			if ((value1 >= (long)Math.Pow(10, 15)) || value1 < 0)
				return "";
			var sb1 = new StringBuilder();
			int r1;
			int p1 = 0;
			while (value1 > 0)
			{
				r1 = (int)(value1 % 1000);
				value1 /= 1000;
				switch (p1)
				{
					case 12:
						sb1.Insert(0, _makeText(r1,
							const_100s,
							const_10s,
							const_3to19,
							const_2_male,
							const_1_male,
							const_1000000000000s));
						break;
					case 9:
						sb1.Insert(0, _makeText(r1,
							const_100s,
							const_10s,
							const_3to19,
							const_2_male,
							const_1_male,
							const_1000000000s));
						break;
					case 6:
						sb1.Insert(0, _makeText(r1,
							const_100s,
							const_10s,
							const_3to19,
							const_2_male,
							const_1_male,
							const_1000000s));
						break;
					case 3:
						switch (textCase)
						{
							case TextCasesEnum.Accusative:
								sb1.Insert(0, _makeText(r1,
									const_100s,
									const_10s,
									const_3to19,
									const_2_female,
									const_1_female_accusative,
									const_1000s_accusative));
								break;
							default:
								sb1.Insert(0, _makeText(r1,
									const_100s,
									const_10s,
									const_3to19,
									const_2_female,
									const_1_female,
									const_1000s));
								break;
						}
						break;
					default:
						string[] a1 = [];
						switch (textCase)
						{
							case TextCasesEnum.Genitive:
								sb1.Insert(0, _makeText(r1,
									const_100s_genetive,
									const_10s_genetive,
									const_3to19_genetive,
									((isMale) ? const_2_male_genetive : const_2_female_genetive),
									((isMale) ? const_1_male_genetive : const_1_female), a1));
								break;
							case TextCasesEnum.Accusative:
								sb1.Insert(0, _makeText(r1,
									const_100s,
									const_10s,
									const_3to19,
									((isMale) ? const_2_male : const_2_female),
									((isMale) ? const_1_male : const_1_female_accusative), a1));
								break;
							default:
								sb1.Insert(0, _makeText(r1,
									const_100s,
									const_10s,
									const_3to19,
									((isMale) ? const_2_male : const_2_female),
									((isMale) ? const_1_male : const_1_female), a1));
								break;
						}
						break;
				}
				p1 += 3;
			}
			var s1 = (value == 0)
				? const_0
				: sb1.ToString().Trim();
			return (!string.IsNullOrEmpty(s1) && firstCapital)
				? string.Concat(s1[..1].ToUpper(), s1.AsSpan(1))
				: s1;
		}


		/// <summary>
		/// Возвращает значение дробного числа прописью
		/// </summary>
		public static string GetWordsRu(
			double source,
			int power,
			TextCasesEnum textCase = TextCasesEnum.Nominative,
			bool firstCapital = false)
		{
			double p1 = Math.Pow(10, power);
			long l1 = (long)Math.Round(source * p1) % (long)p1;
			var s1 = GetWordsRu((long)source, textCase, true, firstCapital);
			var s2 = GetWordsRu(l1, textCase, true, false);
			return $"{s1} целых {s2}".Trim();
		}


		/// <summary>
		/// Возвращает сумму в рублях прописью
		/// </summary>
		public static string GetAmountInRublesInWords(
			double amount,
			bool firstCapital)
		{
			var rub1 = (long)Math.Floor(amount);
			var cop1 = ((long)Math.Round(amount * 100)) % 100;
			int lastRubles1 = _getLastDigit(rub1);
			int lastCopecks1 = _getLastDigit(cop1);
			var sb1 = new StringBuilder();
			sb1.AppendFormat("{0} ",
				GetWordsRu(rub1, TextCasesEnum.Nominative, true, firstCapital));
			if (_isPluralGenitive(lastRubles1))
				sb1.AppendFormat("{0} ", const_rubles[3]);
			else if (_isSingularGenitive(lastRubles1))
				sb1.AppendFormat("{0} ", const_rubles[2]);
			else
				sb1.AppendFormat("{0} ", const_rubles[1]);
			sb1.AppendFormat("{0:00} ", cop1);
			if (_isPluralGenitive(lastCopecks1))
				sb1.AppendFormat("{0} ", const_copecks[3]);
			else if (_isSingularGenitive(lastCopecks1))
				sb1.AppendFormat("{0} ", const_copecks[2]);
			else
				sb1.AppendFormat("{0} ", const_copecks[1]);
			return sb1.ToString().Trim();
		}


		/* privates */


		private const string const_0 = "ноль";
		private const string const_1_male = "один";
		private const string const_1_male_genetive = "одно";
		private const string const_1_female = "одна";
		private const string const_1_female_accusative = "одну";
		private const string const_2_male = "два";
		private const string const_2_male_genetive = "двух";
		private const string const_2_female = "две";
		private const string const_2_female_genetive = "двух";

		private static readonly string[] const_3to19
			= ["", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять", "десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"];

		private static readonly string[] const_3to19_genetive
			= ["", "трех", "четырех", "пяти", "шести", "семи", "восеми", "девяти", "десяти", "одиннадцати", "двенадцати", "тринадцати", "четырнадцати", "пятнадцати", "шестнадцати", "семнадцати", "восемнадцати", "девятнадцати"];

		private static readonly string[] const_10s
			= ["", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто"];

		private static readonly string[] const_10s_genetive
			= ["", "двадцати", "тридцати", "сорока", "пятидесяти", "шестидесяти", "семидесяти", "восьмидесяти", "девяноста"];

		private static readonly string[] const_100s
			= ["", "сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"];

		private static readonly string[] const_100s_genetive
			= ["", "ста", "двухсот", "трехсот", "четырехсот", "пятисот", "шестисот", "семисот", "восемисот", "девятисот"];

		private static readonly string[] const_1000s
			= ["", "тысяча", "тысячи", "тысяч"];

		private static readonly string[] const_1000s_accusative
			= ["", "тысячу", "тысячи", "тысяч"];

		private static readonly string[] const_1000000s
			= ["", "миллион", "миллиона", "миллионов"];

		private static readonly string[] const_1000000000s
			= ["", "миллиард", "миллиарда", "миллиардов"];

		private static readonly string[] const_1000000000000s
			= ["", "трилион", "трилиона", "триллионов"];

		private static readonly string[] const_rubles
			= ["", "рубль", "рубля", "рублей"];

		private static readonly string[] const_copecks
			= ["", "копейка", "копейки", "копеек"];


		private static string _makeText(
			int value,
			string[] text100s,
			string[] text10s,
			string[] text3to19,
			string text2,
			string text1,
			string[] powers)
		{
			var sb1 = new StringBuilder();
			int i1 = value;
			if (i1 >= 100)
			{
				sb1.Append(text100s[i1 / 100] + " ");
				i1 %= 100;
			}
			if (i1 >= 20)
			{
				sb1.Append(text10s[i1 / 10 - 1] + " ");
				i1 %= 10;
			}
			if (i1 >= 3)
				sb1.Append(text3to19[i1 - 2] + " ");
			else if (i1 == 2)
				sb1.Append(text2 + " ");
			else if (i1 == 1)
				sb1.Append(text1 + " ");
			if (value != 0 && powers.Length > 0)
			{
				i1 = _getLastDigit(value);
				if (_isPluralGenitive(i1))
					sb1.Append(powers[3] + " ");
				else if (_isSingularGenitive(i1))
					sb1.Append(powers[2] + " ");
				else
					sb1.Append(powers[1] + " ");
			}
			return sb1.ToString();
		}


		private static bool _isPluralGenitive(
			int value)
		{
			return (value == 0 || value > 4);
		}


		private static bool _isSingularGenitive(
			int value)
		{
			return (value > 1 && value < 5);
		}


		private static int _getLastDigit(
			long value)
		{
			return (int)(value switch
			{
				> 99 => value % 100,
				> 19 => value % 10,
				_ => value
			});
		}

	}

}
