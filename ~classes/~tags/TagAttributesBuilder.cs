namespace Ans.Net10.Common
{

	public class TagAttributesBuilder
	{

		/* ctors */


		public TagAttributesBuilder()
		{
		}


		public TagAttributesBuilder(
			string serialization)
			: this()
		{
			Append(serialization);
		}


		/* readonly properties */


		public Dictionary<string, string> Items { get; } = [];


		/* methods */


		public void Append(
			string serialization)
		{
			if (string.IsNullOrEmpty(serialization))
				return;
			foreach (var item1 in _getItems(serialization))
			{
				(var key, var value) = _getParts(item1);
				Items[key] = value;
			}
		}


		public void AppendIf(
			bool check,
			string serialization)
		{
			if (check)
				Append(serialization);
		}


		/* functions */


		public override string ToString()
		{
			return " " + string.Join(
				" ", Items.Select(
					x => x.GetSerialization("=\"{0}\"")));
		}


		/* privates */


		private static string[] _getItems(
			string serialization)
		{
			return serialization.Split(" ",
				StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
		}


		private static (string key, string value) _getParts(
			string serialization)
		{
			var i1 = serialization.LastIndexOf('=');
			if (i1 == -1)
				return (serialization, null);
			return (serialization[..i1].Trim(), serialization[i1..].Trim().Trim('"'));
		}

	}

}
