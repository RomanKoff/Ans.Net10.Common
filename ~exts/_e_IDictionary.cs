namespace Ans.Net10.Common
{

	public static partial class _e_IDictionary
	{

		/* functions */


		public static T Get<T>(
			this IDictionary<string, T> dictionary,
			string key)
		{
			return (dictionary.TryGetValue(key, out var value2))
				? value2 : default;
		}


		public static string GetValueStringOrKey<T>(
			this IDictionary<string, T> dictionary,
			string key,
			Func<T, string> func,
			string keyTemplate = null)
		{
			return func(dictionary.Get(key)) ?? ((keyTemplate == null)
				? key : string.Format(keyTemplate, key));
		}


		public static string GetString(
			this IDictionary<string, object> dictionary,
			string key,
			string defaultValue)
		{
			return dictionary.TryGetValue(key, out object value1)
				 ? value1?.ToString() : defaultValue;
		}


		public static string GetString(
			this IDictionary<int, string> dictionary,
			int key,
			string defaultValue)
		{
			return dictionary.TryGetValue(key, out string value1)
				? value1 : defaultValue;
		}


		public static int GetInt(
			this IDictionary<string, object> dictionary,
			string key,
			int defaultValue)
		{
			return dictionary.TryGetValue(key, out object value1)
				? value1.ToString().ToInt(defaultValue) : defaultValue;
		}


		public static int GetInt(
			this IDictionary<int, object> dictionary,
			int key,
			int defaultValue)
		{
			return dictionary.TryGetValue(key, out object value1)
				? value1.ToString().ToInt(defaultValue) : defaultValue;
		}


		public static bool GetBool(
			this IDictionary<string, object> dictionary,
			string key,
			bool defaultValue = false)
		{
			return dictionary.TryGetValue(key, out object value1)
				? value1.ToString().ToBool() : defaultValue;
		}


		public static bool GetBool(
			this IDictionary<int, object> dictionary,
			int key,
			bool defaultValue = false)
		{
			return dictionary.TryGetValue(key, out object value1)
				? value1.ToString().ToBool() : defaultValue;
		}

	}

}
