// rev 2026-09-16

using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для работы с рефлексией и метаданными типов.
	/// </summary>
	public static class SuppReflection
	{

		/* functions */


		/// <summary>
		/// Преобразует системное имя встроенного типа CTS в его стандартный C# эквивалент (алиас).
		/// </summary>
		/// <param name="name">Исходное имя типа (например, "Int32").</param>
		/// <returns>Строковый алиас типа в C# (например, "int").</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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


		/// <summary>
		/// Возвращает строковое представление значения объекта, адаптированное под синтаксис C# или отладочный вывод.
		/// </summary>
		/// <param name="value">Объект для анализа.</param>
		/// <returns>Строковое описание или значение объекта.</returns>
		public static string GetCSharpValue(
			object? value,
			bool useFullView = false)
		{
			if (value == null)
				return "null";
			var type1 = value.GetType();
			var name1 = type1.Name;
			return name1 switch
			{
				"String" => (string)value == string.Empty
					? "Empty" : $"\"{value}\"",
				"HtmlString" => value.ToString() == string.Empty
					? "HtmlEmpty" : $"~{value}~",
				"Int32" or
				"Int64" or
				"Single" or
				"Double" or
				"Decimal" => value.ToString()!,
				"Boolean" => (bool)value
					? "true" : "false",
				_ => useFullView
					? _getCSharpValueFull(value, name1, type1)
					: _getCSharpValueShort(value, name1, type1)
			};
		}


		/// <summary>
		/// Возвращает упорядоченный массив типов, принадлежащих указанному пространству имен в заданной сборке.
		/// </summary>
		/// <param name="assembly">Исследуемая сборка объектов.</param>
		/// <param name="ns">Целевое пространство имен.</param>
		/// <returns>Массив объектов <see cref="Type"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Type[] GetNamespaceTypes(
			Assembly assembly,
			string ns)
		{
			return [.. assembly.GetTypes()
				.Where(x => string.Equals(x.Namespace, ns, StringComparison.Ordinal))
				.OrderBy(x => x.Name)];
		}


		/* privates */


		private static string _getCSharpValueFull(
			object value,
			string name,
			Type type)
		{
			if (type.IsEnum)
				return $"{Convert.ChangeType(value, Enum.GetUnderlyingType(type))}.{value}";
			if (value is DateTime dateTime1)
				return dateTime1.ToString("G");
			if (value is DateOnly dateOnly)
				return dateOnly.ToString();
			if (value is TimeOnly timeOnly)
				return timeOnly.ToString("HH:mm:ss");
			if (type.IsArray
				|| value is ICollection
				|| value is IDictionary)
			{
				if (value is ICollection collection)
					return $"[{collection.Count}]";
				if (value is IDictionary dictionary)
					return $"[{dictionary.Count}]";
				if (value is IEnumerable enumerable)
				{
					int count1 = 0;
					var enumerator1 = enumerable.GetEnumerator();
					while (enumerator1.MoveNext())
						count1++;
					return $"[{count1}]";
				}
			}
			if (!name.Contains('`'))
				return $"[{value}]";
			if (name.Contains("Dictionary"))
				return $"[{name}]";
			if (value is IEnumerable enumFallback1)
			{
				int count1 = 0;
				var enumerator1 = enumFallback1.GetEnumerator();
				while (enumerator1.MoveNext())
					count1++;
				return $"[{count1}]";
			}
			return $"[{name}]";
		}


		private static string _getCSharpValueShort(
			object value,
			string name,
			Type type)
		{
			if (type.IsEnum)
				return $"{Convert.ChangeType(value, Enum.GetUnderlyingType(type))}.{value}";
			if (value is DateTime dateTime1)
				return dateTime1.ToString("G");
			if (value is DateOnly dateOnly)
				return dateOnly.ToString();
			if (value is TimeOnly timeOnly)
				return timeOnly.ToString("HH:mm:ss");
			if (type.IsArray
				|| value is ICollection
				|| value is IDictionary)
			{
				if (value is ICollection collection)
					return $"[{collection.Count}]";
				if (value is IDictionary dictionary)
					return $"[{dictionary.Count}]";
				if (value is IEnumerable enumerable)
				{
					int count1 = 0;
					var enumerator1 = enumerable.GetEnumerator();
					while (enumerator1.MoveNext())
						count1++;
					return $"[{count1}]";
				}
			}
			if (!name.Contains('`'))
				return "[object]";
			if (name.Contains("Dictionary"))
				return "[dict]";
			if (value is IEnumerable enumFallback)
			{
				int count1 = 0;
				var enumerator1 = enumFallback.GetEnumerator();
				while (enumerator1.MoveNext())
					count1++;
				return $"[{count1}]";
			}
			return "[object]";
		}

	}

}
