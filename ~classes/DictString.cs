namespace Ans.Net10.Common
{

	public class DictString
		: _Dict_Proto<string, string>
	{

		/* ctors */


		public DictString()
			: base()
		{
		}


		public DictString(
			string serialization)
			: base(serialization)
		{
		}


		public DictString(
			IEnumerable<string> serialization)
			: base(serialization)
		{
		}


		/* overrides */


		public override string StringToKey(
			string key)
		{
			return key;
		}


		public override string StringToValue(
			string value)
		{
			return value;
		}


		public override string KeyToString(
			string key)
		{
			return key;
		}


		public override string ValueToString(
			string value)
		{
			return value;
		}

	}

}
