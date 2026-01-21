namespace Ans.Net10.Common
{

	public class ParamsBuilder
	{

		/* readonly properties */


		public ParamsCollection Parameters { get; } = new();


		/* methods */


		public void Append(
			string name,
			string value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			bool value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			int value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			long value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			double value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			float value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			decimal value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			DateTime? value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			DateOnly? value)
		{
			Parameters.Append(name, value);
		}


		public void Append(
			string name,
			TimeOnly? value)
		{
			Parameters.Append(name, value);
		}


		/* functions */


		public string GetString(
			string additionName,
			string additionValue)
		{
			var a1 = new ParamsCollection(Parameters.Items.Count, Parameters.Items.Comparer);
			foreach (var item1 in Parameters.Items)
				a1.Append(item1.Key, (string)item1.Value.Clone());
			a1.Append(additionName, additionValue);
			return a1.ToString();
		}


		public string GetString(
			string additionName,
			int additionValue)
		{
			return GetString(additionName, additionValue.ToString());
		}


		public override string ToString()
		{
			return Parameters.ToString();
		}

	}

}
