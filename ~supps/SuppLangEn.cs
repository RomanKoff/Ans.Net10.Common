namespace Ans.Net10.Common
{

	public static class SuppLangEn
	{

		/* functions */


		public static string GetPluralizeEn(
			string word)
		{
			// https://engblog.ru/app/uploads/2009/12/Plurals-in-English-table.pdf
			// ay = ays
			// ey = eys
			// oy = oys
			// sh = shes
			// ch = ches
			// y = ies
			// s = ses
			// x = xes
			// +s
			if (word.Length < 3)
				return $"{word}";
			var lastTwo1 = word[^2..];
			var withoutLastTwo1 = word[..^2];
			var withoutLastOne1 = word[..^1];
			return lastTwo1 switch
			{
				"ay" => $"{withoutLastTwo1}ays",
				"ey" => $"{withoutLastTwo1}eys",
				"oy" => $"{withoutLastTwo1}oys",
				"sh" => $"{withoutLastTwo1}shes",
				"ch" => $"{withoutLastTwo1}ches",
				_ => lastTwo1[1] switch
				{
					'y' => $"{withoutLastOne1}ies",
					's' => $"{withoutLastOne1}ses",
					'x' => $"{withoutLastOne1}xes",
					_ => $"{word}s"
				}
			};
			//var ps1 = PluralizationService.CreateService(new CultureInfo("en-gb"));
			//return ps1.Pluralize(word);
		}


		/// <summary>
		/// Возвращает склонение для числа
		/// </summary>
		public static string GetDeclineEn(
			int number,
			string for1,
			string forOther)
		{
			return (number % 10) switch
			{
				1 => for1,
				_ => forOther
			};
		}


		/// <summary>
		/// Возвращает склонение для числа по шаблону
		/// </summary>
		public static string GetDeclineEn(
			string template,
			int number,
			string for1,
			string forOther)
		{
			var s1 = GetDeclineEn(number, for1, forOther);
			return string.Format(template, number, s1);
		}

	}

}
