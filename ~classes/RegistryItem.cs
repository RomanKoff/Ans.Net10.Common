namespace Ans.Net10.Common
{

	public class RegistryItem
	{

		/* ctors */


		public RegistryItem()
		{
		}


		public RegistryItem(
			string key,
			string value,
			int level,
			bool isLabel)
		{
			Key = key;
			Value = value;
			Level = level;
			IsLabel = isLabel;
		}


		/// <summary>
		/// Десериализация элемента из строки
		/// "key=[#][:0]value".
		/// Символы ';=#:' экранируются '\'
		/// </summary>
		public RegistryItem(
			string serialization)
			: this()
		{
			FillFromString(serialization);
		}


		/* readonly properties */


		public string Key { get; private set; }
		public bool IsLabel { get; private set; }
		public int Level { get; private set; }


		/* properties */


		public string Value
		{
			get => _value;
			set => _value = string.IsNullOrEmpty(value)
				? $"{{{Key}}}" : value;
		}
		private string _value;


		/* functions */


		/// <summary>
		/// Возвращает строку сериализации элемента
		/// "key=[#][:0]value".
		/// Символы ';=#:' экранируются '\'
		/// </summary>
		public override string ToString()
		{
			var key1 = SuppString.GetParamEncode(Key);
			var value1 = SuppString.GetParamEncode(Value);
			var label1 = IsLabel.Make("#");
			var level1 = (Level is > 0 and < 10).Make($":{Level}");
			return $"{key1}={label1}{level1}{value1}";
		}


		/* methods */


		/// <summary>
		/// Десериализация элемента из строки
		/// "key=[#][:0]value".
		/// Символы ';=#:' экранируются '\'
		/// </summary>
		public void FillFromString(
			string serialization)
		{
			var s1 = SuppString.GetParamMask(serialization);
			var i1 = s1.IndexOf('=');
			if (i1 < 1)
				throw new ArgumentException(null, nameof(serialization));
			Key = SuppString.GetParamDemask(s1[..i1]);
			var s2 = s1[(i1 + 1)..];
			if (s2[0] == '#')
			{
				IsLabel = true;
				s2 = s2[1..];
			}
			if (s2[0] == ':')
			{
				Level = s2[1].ToString().ToInt(0);
				s2 = s2[2..];
			}
			Value = SuppString.GetParamDemask(s2);
		}

	}

}
