using System.Text;

namespace Ans.Net10.Common
{

	public class OrderItem
	{
		public OrderItem(
			string order)
		{
			if (order[0] == '-')
			{
				IsDescending = true;
				Column = order[1..];
			}
			else
				Column = order;
		}

		public string Column { get; }
		public bool IsDescending { get; }

		public override string ToString()
			=> $"{IsDescending.Make("-")}{Column}";
	}



	public class OrderBuilder
	{

		private readonly List<OrderItem> _items = [];


		/* ctors */


		public OrderBuilder(
			string order)
		{
			if (!string.IsNullOrEmpty(order))
			{
				var a1 = order.Split(
					_Consts.SEPS_ARRAY, StringSplitOptions.RemoveEmptyEntries);
				foreach (var item1 in a1)
					_items.Add(new OrderItem(item1));
			}
		}


		/* readonly properties */


		public OrderItem[] Items
			=> [.. _items];


		/* functions */


		public override string ToString()
		{
			return string.Join(",", _items.Select(x => x.ToString()));
		}


		public string GetLinqCode(
			string ofset)
		{
			var sb1 = new StringBuilder();
			var s1 = (_items[0].IsDescending)
				? $"\n{ofset}.OrderByDescending(x => x.{_items[0].Column})"
				: $"\n{ofset}.OrderBy(x => x.{_items[0].Column})";
			sb1.Append(s1);
			foreach (var item1 in _items.Skip(1))
			{
				var s2 = (item1.IsDescending)
				? $"\n{ofset}.ThenByDescending(x => x.{item1.Column})"
				: $"\n{ofset}.ThenBy(x => x.{item1.Column})";
				sb1.Append(s2);
			}
			return sb1.ToString();
		}

	}

}
