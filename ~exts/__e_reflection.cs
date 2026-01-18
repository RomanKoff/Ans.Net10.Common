using System.Reflection;
using System.Text;

namespace Ans.Net10.Common
{

	public static partial class __e_reflection
	{

		/* functions */


		public static T GetAttribute<T>(
			this Type type)
			where T : Attribute
		{
			return (T)Attribute.GetCustomAttribute(type, typeof(T));
		}


		public static object GetValue(
			this MemberInfo info,
			object forObject)
		{
			return info.MemberType switch
			{
				MemberTypes.Field => ((FieldInfo)info).GetValue(forObject),
				MemberTypes.Property => ((PropertyInfo)info).GetValue(forObject),
				_ => throw new NotImplementedException(),
			};
		}


		public static string GetCSharpTypeName(
			this Type type,
			bool IsDropNullable = false)
		{
			var name1 = type.Name;

			if (name1.EndsWith("[]"))
				return $"{SuppReflection.FixCSharpName(name1[..^2])}[]";

			if (name1.StartsWith("Nullable`"))
				return $"{Nullable.GetUnderlyingType(type)?.GetCSharpTypeName(IsDropNullable)}{IsDropNullable.Make(null, "?")}";

			int i1 = name1.IndexOf('`');

			if (i1 < 0)
				return $"{SuppReflection.FixCSharpName(name1)}";

			var a1 = type.GenericTypeArguments.GetCSharpTypeNames(IsDropNullable);
			return $"{name1[..i1]}<{a1.MakeFromCollection(null, null, ", ")}>";
		}


		public static IEnumerable<string> GetCSharpTypeNames(
			this Type[] types,
			bool IsDropNullable = false)
		{
			return types.Select(
				x => x.GetCSharpTypeName(IsDropNullable));
		}


		public static string GetCSharpGenerics(
			this MethodBase info)
		{
			if (!info.IsGenericMethod)
				return null;
			var a1 = GetCSharpTypeNames(info.GetGenericArguments());
			return $"<{a1.MakeFromCollection(null, null, ", ")}>";
		}


		public static IEnumerable<string> GetCSharpParams(
			this MethodBase info)
		{
			var a1 = new List<string>();
			foreach (var item1 in info.GetParameters())
			{
				var sb1 = new StringBuilder();
				if (item1.GetCustomAttribute<ParamArrayAttribute>() != null)
					sb1.Append("params ");
				sb1.Append($"{item1.ParameterType.GetCSharpTypeName()} {item1.Name}");
				if (item1.HasDefaultValue)
					sb1.Append($" = {SuppReflection.GetCSharpValue(item1.DefaultValue)}");
				a1.Add(sb1.ToString());
			}
			return a1;
		}


		public static string GetPropertyName(
			this MethodInfo info)
		{
			return info.Name.StartsWith("get_")
				? info.Name[4..] : info.Name.StartsWith("set_")
					? info.Name[4..] : info.Name;
		}


		public static object GetPropertyValue(
			this object obj,
			string name,
			Type type)
		{
			return type.GetProperty(name).GetValue(obj, null);
		}


		public static object GetPropertyValue(
			this object obj,
			string name)
		{
			return obj.GetPropertyValue(name, obj.GetType());
		}


		public static T GetPropertyValue<T>(
			this object obj,
			string name,
			Type type)
		{
			return (T)obj.GetPropertyValue(name, type);
		}


		public static T GetPropertyValue<T>(
			this object obj,
			string name)
		{
			return (T)obj.GetPropertyValue(name);
		}


		public static T DefaultObject<T>(
			this object value,
			T defaultValue)
		{
			if (value == null)
				return defaultValue;
			return (T)value;
		}


		public static T DefaultObject<T>(
			this object value)
		{
			return value.DefaultObject<T>(default);
		}

	}

}
