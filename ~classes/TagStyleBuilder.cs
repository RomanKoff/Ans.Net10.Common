namespace Ans.Net10.Common
{

	public class TagStyleBuilder
	{

		private readonly Dictionary<string, string> _styles = [];


		/* ctors */


		public TagStyleBuilder()
		{
		}


		public TagStyleBuilder(
			string serialization)
			: this()
		{
			Append(serialization);
		}


		/* readonly properties */


		public int Count
			=> _styles.Count;


		/* functions */


		private string _join;
		public override string ToString()
		{
			return _join ??= string.Join(
				"", _styles.Select(x => $"{x.Key}:{x.Value};"));
		}


		public string GetJoin()
		{
			_join = null;
			return ToString();
		}


		public bool IsExists(
			string name)
		{
			return _styles.ContainsKey(name);
		}


		/* methods */


		public void Append(
			string serialization)
		{
			if (!string.IsNullOrEmpty(serialization))
				foreach (var s1 in serialization.Split(';',
					StringSplitOptions.RemoveEmptyEntries
					| StringSplitOptions.TrimEntries))
				{
					var a1 = s1.Split(':');
					AppendIf((a1.Length == 2), a1[0], a1[1]);
				}
		}


		public void Append(
			string name,
			string value)
		{
			if (!string.IsNullOrEmpty(value))
				_styles[name] = value;
		}


		public void Append(
			string name,
			int value,
			int nullValue = 0)
		{
			if (value != nullValue)
				_styles[name] = value.ToString();
		}


		public void AppendIf(
			bool check,
			string name,
			string value)
		{
			if (check)
				Append(name, value);
		}


		public void Remove(
			string name)
		{
			_styles.Remove(name);
		}


		public void Clear()
		{
			_styles.Clear();
		}

	}

}
