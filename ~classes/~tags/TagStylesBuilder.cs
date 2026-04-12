namespace Ans.Net10.Common
{

	public class TagStylesBuilder
	{

		/* ctors */


		public TagStylesBuilder()
		{
		}


		public TagStylesBuilder(
			string cssStyles)
			: this()
		{
			Append(cssStyles);
		}


		/* readonly properties */


		public Dictionary<string, string> Items { get; } = [];


		/* methods */


		public void ApplyOriginal(
			Dictionary<string, string> dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				if (!Items.ContainsKey(item1.Key))
					Items[item1.Key] = item1.Value;
		}


		public void ApplyOriginal(
			string cssStyles)
		{
			if (string.IsNullOrEmpty(cssStyles))
				return;
			ApplyOriginal(_getDict(cssStyles));
		}


		public void Append(
			Dictionary<string, string> dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				Items[item1.Key] = item1.Value;
		}


		public void Append(
			string cssStyles)
		{
			if (string.IsNullOrEmpty(cssStyles))
				return;
			Append(_getDict(cssStyles));
		}


		public void AppendIf(
			bool check,
			string cssStyles)
		{
			if (check)
				Append(cssStyles);
		}


		/* functions */


		public override string ToString()
		{
			return string.Join(
				"", Items.Select(
					x => x.GetSerialization(":{0};")));
		}


		/* privates */


		private static string[] _getItems(
			string cssStyles)
		{
			return cssStyles.Split(";",
				StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		}


		private static (string key, string value) _getParts(
			string cssStyle)
		{
			var a1 = cssStyle.Split(':');
			return a1.Length == 2
				? (a1[0].Trim(), a1[1].Trim())
				: (null, null);
		}


		private static Dictionary<string, string> _getDict(
			string cssStyles)
		{
			var d1 = new Dictionary<string, string>();
			foreach (var item1 in _getItems(cssStyles))
			{
				(var key, var value) = _getParts(item1);
				if (key != null)
					d1[key] = value;
			}
			return d1;
		}

	}

}
