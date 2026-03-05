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
			var items1 = new List<T>();
			var a1 = source.Split(['\r', '\n']);
			foreach (var item1 in a1)
				if (!string.IsNullOrWhiteSpace(item1) && !item1.StartsWith("//"))
					items1.Add(parser(item1));
			return items1.AsEnumerable();
		}


		public static IEnumerable<T> MakeFromArray<T>(
			Func<string, T> parser,
			IEnumerable<string> source)
		{
			return source.Select(x => parser(x));
		}

	}

}
