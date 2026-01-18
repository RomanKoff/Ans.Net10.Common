using System.Reflection;

namespace Ans.Net10.Common
{

	public static class SuppReflection
	{

		/* functions */


		public static string FixCSharpName(
			string name)
		{
			return name switch
			{
				"Boolean" => "bool",
				"Char" => "char",
				"Decimal" => "decimal",
				"Double" => "double",
				"Int32" => "int",
				"Int64" => "long",
				"Single" => "float",
				"String" => "string",
				"Void" => "void",
				_ => name
			};
		}


		public static string GetCSharpValue(
			object value)
		{
			if (value == null)
				return "null";

			var type1 = value.GetType();
			var name1 = type1.Name;

			return name1 switch
			{
				"String" => value.ToString() == string.Empty
					? "Empty"
					: $"\"{value}\"",
				"HtmlString" => value.ToString() == string.Empty
					? "HtmlEmpty"
					: $"~{value}~",
				"Int32" or
				"Int64" or
				"Single" or
				"Double" or
				"Decimal" => $"{value}",
				"Boolean" => (bool)value
					? "true"
					: "false",
				_ => _getCSharpSetValue(value, name1, type1)
			};
		}


		public static Type[] GetNamespaceTypes(
			Assembly assembly,
			string ns)
		{
			return [.. assembly.GetTypes()
				.Where(x => string.Equals(x.Namespace, ns, StringComparison.Ordinal))
				.OrderBy(x => x.Name)];
		}


		/* privates */


		private static string _getCSharpSetValue(
			object value,
			string name,
			Type type)
		{
			if (type.IsEnum)
			{
				var value1 = Convert.ChangeType(value, Enum.GetUnderlyingType(type));
				return $"[{value1}]";
			}
			if (name.Contains("[]"))
				return $"[{((IEnumerable<object>)value).Count()}]";
			if (!name.Contains('`'))
				return "[object]"; // todo date time
			if (name.Contains("Dictionary"))
				return "[dict]";
			return $"[({((IEnumerable<object>)value).Count()})]";
		}

	}

}
