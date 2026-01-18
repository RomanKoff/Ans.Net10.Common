namespace Ans.Net10.Common
{

	public class DictInt
		: _Dict_Proto<int, string>
	{

		/* ctors */


		public DictInt()
			: base()
		{
		}


		public DictInt(
			string serialization)
			: base(serialization)
		{
		}


		public DictInt(
			IEnumerable<string> serialization)
			: base(serialization)
		{
		}


		/* overrides */


		public override int StringToKey(
			string key)
		{
			return key.ToInt()
				?? throw new NotImplementedException();
		}


		public override string StringToValue(
			string value)
		{
			return value;
		}


		public override string KeyToString(
			int key)
		{
			return key.ToString();
		}


		public override string ValueToString(
			string value)
		{
			return value;
		}

	}

}
