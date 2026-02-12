using System.Text;

namespace Ans.Net10.Common
{

	public enum TextCaseEnum
	{
		Original,
		Lower,
		Upper,
		FirstUpper,
		StartWithACapital
	}



	public enum GenderEnum
		: int
	{
		NotSpecified = 0, // не указан
		Male = 1, // мужской
		Female = 2, // женский
	}



	public static class SuppValues
	{

		/* functions */


		public static string Default(
			string current,
			params string[] defaultValues)
		{
			if (!string.IsNullOrEmpty(current))
				return current;
			foreach (var value1 in defaultValues)
				if (!string.IsNullOrEmpty(value1))
					return value1;
			return null;
		}


		public static int Default(
			int current,
			int defaultValue,
			int nullValue = 0)
		{
			return (current == nullValue)
				? defaultValue : current;
		}


		private static readonly char[] _famioSeps = [' ', '.', ','];


		/// <summary>
		/// Возвращает фамилию и инициалы из строки содержащей фамилию имя и отчество
		/// (Пушкин Александр Сергеевич, Салтыков-Щедрин Михаил Евграфович, Эфендиев Эльчин Ильяс оглы)
		/// [Пушкин А.С., Салтыков-Щедрин М.Е., Эфендиев Э.И.]
		/// </summary>
		/// <param name="fullname">Исходная строка</param>
		/// <param name="textCase">Операция преобразования регистра букв</param>
		public static (string family, string initials) GetFamilyAndInitials(
			string fullname,
			TextCaseEnum textCase = TextCaseEnum.StartWithACapital)
		{
			if (string.IsNullOrEmpty(fullname))
				return (null, null);
			var a1 = fullname.Split(_famioSeps,
				StringSplitOptions.RemoveEmptyEntries);
			var family1 = SuppString.GetModCase(a1[0], textCase);
			var sb1 = new StringBuilder();
			if (a1.Length > 1)
				sb1.Append($"{a1[1].First()}.");
			if (a1.Length > 2)
				sb1.Append($"{a1[2].First()}.");
			var initials1 = SuppString.GetModCase(sb1.ToString(), textCase);
			return (family1, initials1);
		}


		/// <summary>
		/// Возвращает строку содержащую фамилию и инициалы из строки содержащей фамилию имя и отчество
		/// (Пушкин Александр Сергеевич, Салтыков-Щедрин Михаил Евграфович, Эфендиев Эльчин Ильяс оглы)
		/// [Пушкин А.С., Салтыков-Щедрин М.Е., Эфендиев Э.И.]
		/// </summary>
		/// <param name="fullname">Фамилию имя и отчество</param>
		/// <param name="textCase">Операция преобразования регистра букв</param>
		public static string GetFamilyAndInitialsString(
			string fullname,
			TextCaseEnum textCase = TextCaseEnum.StartWithACapital)
		{
			var (family1, initials1) = GetFamilyAndInitials(fullname, textCase);
			return $"{family1}{initials1.Make(" {0}")}";
		}


		/// <summary>
		/// Возвращает строку содержащую латинскую транслитерацию для использования в идентификаторе
		/// ГОСТ Р 7.0.34-2014
		/// (https://www.ifap.ru/library/gost/70342014.pdf)
		/// (Пушкин Александр Сергеевич, Салтыков-Щедрин Михаил Евграфович, Эфендиев Эльчин Ильяс оглы)
		/// [pushkin_as, saltikovtschedrin_me, efendiev_ei]
		/// </summary>
		public static string GetFamilyAndInitialsTranslit(
			string family,
			string initials)
		{
			var sb1 = new StringBuilder($"{family.Replace("-", "")}");
			if (!string.IsNullOrEmpty(initials))
				sb1.Append($"_{initials.Replace(".", "")}");
			return SuppLangRu.GetTranslitRuToEn(sb1.ToString().ToLower());
		}


		/// <summary>
		/// Возвращает пол по фамилии имени и отчеству
		/// (Пушкин Александр Сергеевич, Эфендиев Эльчин Ильяс оглы)
		/// [pushkin_as, saltikovtschedrin_me, efendiev_ei]
		/// </summary>
		public static GenderEnum GetGender(
			string fullname)
		{
			if (string.IsNullOrEmpty(fullname))
				return GenderEnum.NotSpecified;
			if (fullname.EndsWith("ич"))
				return GenderEnum.Male;
			if (fullname.EndsWith("на"))
				return GenderEnum.Female;
			if (fullname.EndsWith("глы"))
				return GenderEnum.Male;
			if (fullname.EndsWith("ызы"))
				return GenderEnum.Female;
			return GenderEnum.NotSpecified;
		}


		public static string FixTelephoneRuCityCode(
			string phone)
		{
			if (string.IsNullOrEmpty(phone))
				return null;
			return phone[0] == '8'
				? $"7{phone[1..]}" : phone;
		}


		public static string GetDocNumber(
			string number)
		{
			var sb1 = new StringBuilder();
			foreach (var ch1 in number)
			{
				var code1 = (int)ch1;
				if (code1 > 47 && code1 < 58)
					sb1.Append(ch1);
				else
					sb1.Append('-');
			}
			return sb1.ToString()
				.GetReplaceRecursively("--", "-")
				.Trim('-');
		}


		public static string GetTelephoneNumber(
			string number)
		{
			if (string.IsNullOrEmpty(number))
				return null;
			int l1 = number.Length;
			if (l1 > 11 || l1 < 5)
				return number;
			var stops1 = new int[] { l1 - 2, l1 - 4, l1 - 7, l1 - 10 };
			var a1 = new char[16];
			int p1 = 1;
			a1[0] = number[0];
			for (int i1 = 1; i1 < l1; i1++)
			{
				if (stops1.Contains(i1))
				{
					a1[p1] = '-';
					p1++;
				}
				a1[p1] = number[i1];
				p1++;
			}
			return $"+{new string(a1, 0, p1)}";
		}


		public static string GetValueStringForWeb(
			object value)
		{
			if (value == null)
				return null;
			var type1 = value.GetType().ToString();
			return type1 switch
			{
				"System.DateTime"
					=> ((DateTime)value).ToString("u"),
				"System.DateOnly"
					=> ((DateOnly)value).ToString("yyyy-MM-dd"),
				"System.TimeOnly"
					=> ((TimeOnly)value).ToString("HH\\:mm\\:ss.fff"),
				"System.Boolean"
					=> ((bool)value).Make("true", "false"),
				_ => $"{value}",
			};
		}


		public static string GetDigitalOnly(
			string number)
		{
			return _Consts.G_REGEX_NOT_NUMBER().Replace(number, "");
		}


		public static string GetCurrencyLoc(
			float amount)
		{
			return string.Format("{0:N2}", amount);
		}


		public static string GetCurrencyLoc(
			double amount)
		{
			return string.Format("{0:N2}", amount);
		}


		public static string GetCurrencyBuh(
			float amount)
		{
			long h1 = (long)Math.Floor(amount);
			long h2 = ((long)Math.Round(amount * 100)) % 100;
			return string.Format("{0}={1:00}", h1, h2);
		}


		public static string GetCurrencyBuh(
			double amount)
		{
			long h1 = (long)Math.Floor(amount);
			long h2 = ((long)Math.Round(amount * 100)) % 100;
			return string.Format("{0}={1:00}", h1, h2);
		}

	}

}
