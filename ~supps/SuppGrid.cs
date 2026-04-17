namespace Ans.Net10.Common
{

	public static class SuppGrid
	{

		/* functions */


		public static IEnumerable<T> MakeFromStream<T>(
			Func<string, T> parser,
			Stream source)
		{
			var items1 = new List<T>();
			using var reader1 = new StreamReader(source);
			string line1;
			while ((line1 = reader1.ReadLine()) != null)
				if (!string.IsNullOrWhiteSpace(line1) && !line1.StartsWith("//"))
					items1.Add(parser(line1));
			return items1.AsEnumerable();
		}


		public static IEnumerable<T> MakeFromString<T>(
			Func<string, T> parser,
			string source)
		{
			return source
				.Split(["\r\n", "\r", "\n"], StringSplitOptions.None)
				.Where(x => !string.IsNullOrWhiteSpace(x) && !x.StartsWith("//"))
				.Select(x => parser(x));
		}


		public static IEnumerable<T> MakeFromArray<T>(
			Func<string, T> parser,
			IEnumerable<string> source)
		{
			return source.Select(x => parser(x));
		}

	}

}
