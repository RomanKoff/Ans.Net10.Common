using System.Text;

namespace Ans.Net10.Common
{

	public enum TextCaseEnum
	{
		Original,
		Upper,
		Lower,
		StartWithACapital
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


		/// <summary>
		/// Возвращает фамилию и инициалы из строки содержащей фамилию имя и отчество
		/// </summary>
		/// <param name="fio">Исходная строка</param>
		/// <param name="useDots">Использовать точку после букв инициалов, иначе пробел</param>
		/// <param name="textCase">Операция преобразования регистра букв</param>
		/// <returns></returns>
		public static string GetFamIO(
			string fio,
			bool useDots = true,
			TextCaseEnum textCase = TextCaseEnum.StartWithACapital)
		{
			if (string.IsNullOrEmpty(fio))
				return string.Empty;
			var a1 = fio.Split(_famioSeps,
				StringSplitOptions.RemoveEmptyEntries);
			var sb1 = new StringBuilder(a1[0]);
			if (a1.Length > 1)
			{
				sb1.Append(' ');
				var d1 = useDots ? '.' : ' ';
				foreach (var item1 in a1.Skip(1))
					sb1.Append($"{item1[0]}{d1}");
			}
			var s1 = sb1.ToString();
			return textCase switch
			{
				TextCaseEnum.Upper => s1.ToUpper(),
				TextCaseEnum.Lower => s1.ToLower(),
				TextCaseEnum.StartWithACapital => s1.GetStartWithACapital(),
				_ => s1
			};
		}
		private static readonly char[] _famioSeps = [' ', '.', ',', '-'];


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
