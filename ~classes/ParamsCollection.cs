using System.Text;

namespace Ans.Net10.Common
{

	public class ParamsCollection
	{

		private readonly Dictionary<string, string> _items;


		/* ctors */


		public ParamsCollection()
		{
			_items = [];
		}


		public ParamsCollection(
			int capacity,
			IEqualityComparer<string> comparer)
		{
			_items = new(capacity, comparer);
		}


		/* readonly properties */


		public Dictionary<string, string> Items
			=> _items;


		/* methods */


		public void Append(
			string name,
			string value)
		{
			if (!_items.TryAdd(name, value))
				_items[name] = value;
		}


		public void Append(
			string name,
			bool value)
		{
			if (value)
				Append(name, "1");
		}


		public void Append(
			string name,
			int value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		public void Append(
			string name,
			long value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		public void Append(
			string name,
			double value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		public void Append(
			string name,
			float value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		public void Append(
			string name,
			decimal value)
		{
			if (value != 0)
				Append(name, value.ToString());
		}


		public void Append(
			string name,
			DateTime? value)
		{
			if (value != null)
				Append(name, value?.ToString("u"));
		}


		public void Append(
			string name,
			DateOnly? value)
		{
			if (value != null)
				Append(name, value?.ToString("yyyy-MM-dd"));
		}


		public void Append(
			string name,
			TimeOnly? value)
		{
			if (value != null)
				Append(name, value?.ToString("hh:mm:ss"));
		}


		/* functions */


		public override string ToString()
		{
			if (Items.Count == 0)
				return string.Empty;
			var sb1 = new StringBuilder();
			foreach (var item1 in Items)
				sb1.Append($"&{item1.Key}={item1.Value}");
			return $"?{sb1.ToString()[1..]}";
		}

	}

}