using System.Text;

namespace Ans.Net10.Common
{

	public class TagClassesBuilder
	{

		/* ctors */


		public TagClassesBuilder()
		{
		}


		public TagClassesBuilder(
			string cssClasses)
			: this()
		{
			Append(cssClasses);
		}


		/* readonly properties */


		public Dictionary<string, string[]> Items { get; } = [];


		/* methods */


		public void ApplyOriginal(
			Dictionary<string, string[]> dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				if (!Items.ContainsKey(item1.Key))
					Items[item1.Key] = item1.Value;
		}


		public void ApplyOriginal(
			string cssClasses)
		{
			if (string.IsNullOrEmpty(cssClasses))
				return;
			ApplyOriginal(_getDict(cssClasses));
		}


		public void Append(
			Dictionary<string, string[]> dict)
		{
			if (dict == null)
				return;
			foreach (var item1 in dict)
				Items[item1.Key] = item1.Value;
		}


		public void Append(
			string cssClasses)
		{
			if (string.IsNullOrEmpty(cssClasses))
				return;
			Append(_getDict(cssClasses));
		}


		public void AppendIf(
			bool check,
			string cssClasses)
		{
			if (check)
				Append(cssClasses);
		}


		/* functions */


		public override string ToString()
		{
			if (Items.Count == 0)
				return null;
			var sb1 = new StringBuilder();
			foreach (var item1 in Items)
				foreach (var value1 in item1.Value)
					sb1.Append(" " + item1.Key + value1);
			return sb1.Remove(0, 1).ToString();
		}


		/* privates */


		private static string[] _getItems(
			string cssClasses)
		{
			return cssClasses.Split(" ",
				StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		}


		private static (string key, string value) _getParts(
			string cssClass)
		{
			var i1 = cssClass.LastIndexOf('-');
			if (i1 == -1)
				return (cssClass, null);
			i1++;
			return (cssClass[..i1], cssClass[i1..]);
		}


		private static Dictionary<string, string[]> _getDict(
			string cssClasses)
		{
			var d1 = new Dictionary<string, string[]>();
			foreach (var item1 in _getItems(cssClasses))
			{
				(var key, var value) = _getParts(item1);
				if (d1.TryGetValue(key, out var value1))
					d1[key] = value1.GetArrayAdd(value);
				else
					d1[key] = [value];
			}
			return d1;
		}

	}

}
