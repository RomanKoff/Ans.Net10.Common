// rev 2026-09-17

using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class __e_reflection
	{

		/* functions */


		/// <summary>
		/// Преобразует свойства объекта в динамический объект <see cref="ExpandoObject"/>
		/// на основе дескрипторов типов.
		/// </summary>
		/// <param name="value">Исходный объект.</param>
		/// <returns>Динамическое представление объекта.</returns>
		public static dynamic ToDynamic(
			this object value)
		{
			ArgumentNullException.ThrowIfNull(value);
			IDictionary<string, object?> expando1 = new ExpandoObject();
			foreach (PropertyDescriptor property1 in TypeDescriptor.GetProperties(value.GetType()))
				expando1.Add(property1.Name, property1.GetValue(value));
			return expando1;
		}


		/// <summary>
		/// Возвращает кастомный атрибут указанного типа, примененный к типу данных.
		/// </summary>
		/// <typeparam name="T">Тип искомого атрибута.</typeparam>
		/// <param name="type">Исследуемый тип данных.</param>
		/// <returns>Экземпляр атрибута или <see langword="null"/>, если атрибут не найден.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetAttribute<T>(
			this Type type)
			where T : Attribute
		{
			return (T?)Attribute.GetCustomAttribute(type, typeof(T));
		}


		/// <summary>
		/// Универсально извлекает значение поля или свойства объекта через метаданные члена типа.
		/// </summary>
		/// <param name="info">Информация о члене типа (поле или свойство).</param>
		/// <param name="forObject">Экземпляр объекта, из которого извлекается значение.</param>
		/// <returns>Значение члена типа.</returns>
		/// <exception cref="NotImplementedException">Вызывается, если член типа не является полем или свойством.</exception>
		public static object? GetValue(
			this MemberInfo info,
			object forObject)
		{
			return info.MemberType switch
			{
				MemberTypes.Field => ((FieldInfo)info).GetValue(forObject),
				MemberTypes.Property => ((PropertyInfo)info).GetValue(forObject),
				_ => throw new NotImplementedException(
					$"[Ans.Net10.Common] Extracting a value for the member type {info.MemberType} is not supported.")
			};
		}


		/// <summary>
		/// Возвращает удобочитаемое имя типа в синтаксисе C# (включая массивы, Nullable и Generics).
		/// </summary>
		/// <param name="type">Тип данных.</param>
		/// <param name="IsDropNullable">Признак принудительного удаления знака '?' для Nullable типов.</param>
		/// <returns>Строковое представление имени типа на C#.</returns>
		public static string GetCSharpTypeName(
			this Type type,
			bool IsDropNullable = false)
		{
			ArgumentNullException.ThrowIfNull(type);
			if (type.IsArray)
			{
				var type1 = type.GetElementType();
				return $"{type1?.GetCSharpTypeName(IsDropNullable)}[]";
			}
			var name1 = type.Name;
			if (name1.StartsWith("Nullable`"))
			{
				var type1 = Nullable.GetUnderlyingType(type);
				return $"{type1?.GetCSharpTypeName(IsDropNullable)}{IsDropNullable.Make(string.Empty, "?")}";
			}
			int i1 = name1.IndexOf('`');
			if (i1 < 0)
				return SuppReflection.FixCSharpName(name1);
			var a1 = type.GenericTypeArguments.GetCSharpTypeNames(IsDropNullable);
			return $"{name1[..i1]}<{a1.MakeFromCollection(null, null, ", ")}>";
		}


		/// <summary>
		/// Возвращает коллекцию удобочитаемых имён C# для массива типов.
		/// </summary>
		/// <param name="types">Массив типов данных.</param>
		/// <param name="IsDropNullable">Признак принудительного удаления знака '?' для Nullable типов.</param>
		/// <returns>Перечисление строковых имён типов.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<string> GetCSharpTypeNames(
			this Type[] types,
			bool IsDropNullable = false)
		{
			return types.Select(x => x.GetCSharpTypeName(IsDropNullable));
		}


		/// <summary>
		/// Возвращает строковую запись обобщенных параметров (Generics) метода в стиле C#.
		/// </summary>
		/// <param name="info">Метаданные метода.</param>
		/// <returns>Строка вида &lt;T1, T2&gt; или <see langword="null"/>, если метод не является обобщенным.</returns>
		public static string? GetCSharpGenerics(
			this MethodBase info)
		{
			if (!info.IsGenericMethod)
				return null;
			var a1 = GetCSharpTypeNames(info.GetGenericArguments());
			return $"<{a1.MakeFromCollection(null, null, ", ")}>";
		}


		/// <summary>
		/// Возвращает коллекцию строковых представлений параметров метода в формате объявления C# (включая params и дефолтные значения).
		/// </summary>
		/// <param name="info">Метаданные метода.</param>
		/// <returns>Перечисление строк с параметрами.</returns>
		public static IEnumerable<string> GetCSharpParams(
			this MethodBase info)
		{
			var a1 = new List<string>();
			foreach (var item1 in info.GetParameters())
			{
				string prefix1 = item1.GetCustomAttribute<ParamArrayAttribute>() != null ? "params " : string.Empty;
				string name1 = item1.ParameterType.GetCSharpTypeName();
				string default1 = item1.HasDefaultValue
					? $" = {SuppReflection.GetCSharpValue(item1.DefaultValue)}"
					: string.Empty;
				a1.Add($"{prefix1}{name1} {item1.Name}{default1}");
			}
			return a1;
		}


		/// <summary>
		/// Очищает имя метода доступа к свойству от системных префиксов "get_" или "set_", возвращая чистое имя свойства.
		/// </summary>
		/// <param name="info">Метаданные метода.</param>
		/// <returns>Очищенное имя свойства.</returns>
		public static string GetPropertyName(
			this MethodInfo info)
		{
			string name1 = info.Name;
			if (name1.StartsWith("get_") || name1.StartsWith("set_"))
				return name1[4..];
			return name1;
		}


		/// <summary>
		/// Возвращает значение свойства объекта по его имени на основе указанного типа метаданных.
		/// </summary>
		/// <param name="obj">Экземпляр объекта.</param>
		/// <param name="name">Имя свойства.</param>
		/// <param name="type">Тип данных для поиска свойства.</param>
		/// <returns>Значение свойства объекта.</returns>
		/// <exception cref="InvalidOperationException">Вызывается, если свойство с указанным именем не найдено.</exception>
		public static object? GetPropertyValue(
			this object obj,
			string name,
			Type type)
		{
			var prop1 = type.GetProperty(name)
				?? throw new InvalidOperationException(
					$"The property '{name}' was not found in the type '{type.Name}'.");
			return prop1.GetValue(obj, null);
		}


		/// <summary>
		/// Возвращает значение свойства объекта по его строковому имени на основе рантайм-типа объекта.
		/// </summary>
		/// <param name="obj">Экземпляр объекта.</param>
		/// <param name="name">Имя свойства.</param>
		/// <returns>Значение свойства.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static object? GetPropertyValue(
			this object obj,
			string name)
		{
			return obj.GetPropertyValue(name, obj.GetType());
		}


		/// <summary>
		/// Возвращает типизированное значение свойства объекта по его имени на основе указанного типа метаданных.
		/// </summary>
		/// <typeparam name="T">Целевой тип значения свойства.</typeparam>
		/// <param name="obj">Экземпляр объекта.</param>
		/// <param name="name">Имя свойства.</param>
		/// <param name="type">Тип данных для поиска свойства.</param>
		/// <returns>Типизированное значение свойства.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetPropertyValue<T>(
			this object obj,
			string name,
			Type type)
		{
			return (T)obj.GetPropertyValue(name, type)!;
		}


		/// <summary>
		/// Возвращает типизированное значение свойства объекта по его строковому имени.
		/// </summary>
		/// <typeparam name="T">Целевой тип значения свойства.</typeparam>
		/// <param name="obj">Экземпляр объекта.</param>
		/// <param name="name">Имя свойства.</param>
		/// <returns>Типизированное значение свойства.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T GetPropertyValue<T>(
			this object obj,
			string name)
		{
			return (T)obj.GetPropertyValue(name)!;
		}


		/// <summary>
		/// Возвращает переданное значение, приведенное к типу T, либо альтернативное значение по умолчанию, если исходный объект равен null.
		/// </summary>
		/// <typeparam name="T">Целевой тип объекта.</typeparam>
		/// <param name="value">Проверяемый объект.</param>
		/// <param name="defaultValue">Значение по умолчанию.</param>
		/// <returns>Объект типа T или defaultValue.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T DefaultObject<T>(
			this object? value,
			T defaultValue)
		{
			if (value == null)
				return defaultValue;
			return (T)value;
		}


		/// <summary>
		/// Возвращает переданное значение, приведенное к типу T, либо дефолтное значение типа default(T), если исходный объект равен null.
		/// </summary>
		/// <typeparam name="T">Целевой тип объекта.</typeparam>
		/// <param name="value">Проверяемый объект.</param>
		/// <returns>Объект типа T или дефолтное значение типа.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? DefaultObject<T>(
			this object? value)
		{
			return value.DefaultObject(default(T)!);
		}

	}

}
