// rev 2026-09-19

using System.Reflection;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Базовый класс для описания элементов метаданных объекта.
	/// </summary>
	public class _ObjInfo_Base
	{
		/// <summary>
		/// Возвращает или задает имя элемента (свойства, метода, конструктора).
		/// </summary>
		public string Name { get; set; } = string.Empty;
	}



	/// <summary>
	/// Описание метаданных свойства объекта.
	/// </summary>
	public class ObjInfoProperty
		: _ObjInfo_Base
	{
		/// <summary>
		/// Возвращает или задает признак наличия метода чтения (get).
		/// </summary>
		public bool HasGetter { get; set; }

		/// <summary>
		/// Возвращает или задает признак наличия метода записи (set).
		/// </summary>
		public bool HasSetter { get; set; }

		/// <summary>
		/// Возвращает или задает удобочитаемое имя типа свойства в синтаксисе C#.
		/// </summary>
		public string TypeName { get; set; } = string.Empty;

		/// <summary>
		/// Возвращает или задает строковое представление текущего значения свойства.
		/// </summary>
		public string? Value { get; set; }
	}



	/// <summary>
	/// Описание метаданных метода или конструктора объекта.
	/// </summary>
	public class ObjInfoMethod
		: _ObjInfo_Base
	{
		/// <summary>
		/// Возвращает или задает строковое представление обобщенных параметров (Generics) метода.
		/// </summary>
		public string? Generics { get; set; }

		/// <summary>
		/// Возвращает или задает коллекцию строковых объявлений параметров метода.
		/// </summary>
		public IEnumerable<string> Parameters { get; set; } = [];
	}



	/// <summary>
	/// Описание метаданных метода-функции, возвращающей значение.
	/// </summary>
	public class ObjInfoFunction
		: ObjInfoMethod
	{
		/// <summary>
		/// Возвращает или задает удобочитаемое имя возвращаемого типа данных в синтаксисе C#.
		/// </summary>
		public string Return { get; set; } = string.Empty;
	}



	/// <summary>
	/// Строитель и контейнер структурированных метаданных типов и объектов.
	/// </summary>
	public class ObjInfoBuilder
	{

		/* ctor */

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="ObjInfoBuilder"/> для указанного типа 
		/// и опционально извлекает значения свойств из живого объекта.
		/// </summary>
		/// <param name="type">Исследуемый тип данных.</param>
		/// <param name="obj">
		/// Опциональный экземпляр объекта исследуемого типа для считывания значений свойств.
		/// </param>
		public ObjInfoBuilder(
			Type type,
			object? obj = null)
		{
			InfoType = type;
			_parse();
			if (obj != null)
			{
				foreach (var prop1 in ReadWriteProperties)
					prop1.Value = SuppReflection.GetCSharpValue(obj.GetPropertyValue(prop1.Name));
				foreach (var prop1 in ReadOnlyProperties)
					prop1.Value = SuppReflection.GetCSharpValue(obj.GetPropertyValue(prop1.Name));
			}
		}


		/* readonly properties */


		/// <summary>
		/// Возвращает исследуемый тип данных.
		/// </summary>
		public Type InfoType { get; }


		/// <summary>
		/// Возвращает коллекцию метаданных конструкторов типа.
		/// </summary>
		public IReadOnlyCollection<ObjInfoMethod> Ctors { get; private set; } = [];


		/// <summary>
		/// Возвращает коллекцию свойств, доступных и на чтение, и на запись.
		/// </summary>
		public IReadOnlyCollection<ObjInfoProperty> ReadWriteProperties { get; private set; } = [];


		/// <summary>
		/// Возвращает коллекцию свойств, доступных только для чтения (ReadOnly).
		/// </summary>
		public IReadOnlyCollection<ObjInfoProperty> ReadOnlyProperties { get; private set; } = [];


		/// <summary>
		/// Возвращает коллекцию свойств, доступных только для записи (WriteOnly).
		/// </summary>
		public IReadOnlyCollection<ObjInfoProperty> WriteOnlyProperties { get; private set; } = [];


		/// <summary>
		/// Возвращает коллекцию методов-функций (возвращающих значения).
		/// </summary>
		public IReadOnlyCollection<ObjInfoFunction> Functions { get; private set; } = [];


		/// <summary>
		/// Возвращает коллекцию обычных методов (возвращающих void).
		/// </summary>
		public IReadOnlyCollection<ObjInfoMethod> Methods { get; private set; } = [];


		/// <summary>
		/// Возвращает признак наличия свойств типа ReadWrite.
		/// </summary>
		public bool HasReadWriteProperties
			=> ReadWriteProperties.Count > 0;


		/// <summary>
		/// Возвращает признак наличия свойств типа ReadOnly.
		/// </summary>
		public bool HasReadonlyProperties
			=> ReadOnlyProperties.Count > 0;


		/// <summary>
		/// Возвращает признак наличия свойств типа WriteOnly.
		/// </summary>
		public bool HasWriteonlyProperties
			=> WriteOnlyProperties.Count > 0;


		/// <summary>
		/// Возвращает признак наличия любых свойств у исследуемого типа.
		/// </summary>
		public bool HasProperties
			=> HasReadWriteProperties
				|| HasReadonlyProperties
				|| HasWriteonlyProperties;


		/* privates */


		private void _parse()
		{
			var objectType1 = typeof(object);

			// Парсинг конструкторов
			Ctors = [.. InfoType.GetConstructors()
				.Select(x => new ObjInfoMethod
				{
					Name = x.Name,
					Parameters = x.GetCSharpParams()
				})];

			// Получение всех методов, кроме унаследованных от System.Object
			var methods = InfoType.GetMethods()
				.Where(x => x.DeclaringType != objectType1);

			var getters1 = new List<MethodInfo>();
			var setters1 = new List<MethodInfo>();
			var funcs1 = new List<MethodInfo>();
			var meths1 = new List<MethodInfo>();

			foreach (var item1 in methods)
			{
				if (item1.Name.StartsWith("get_"))
					getters1.Add(item1);
				else if (item1.Name.StartsWith("set_"))
					setters1.Add(item1);
				else if (item1.ReturnType == typeof(void))
					meths1.Add(item1);
				else
					funcs1.Add(item1);
			}

			var propsDict1 = new Dictionary<string, ObjInfoProperty>();

			foreach (var item1 in getters1)
			{
				var prop1 = _makeProp(item1, isGetter: true);
				prop1.HasGetter = true;
				propsDict1[prop1.Name] = prop1;
			}

			foreach (var item1 in setters1)
			{
				var name1 = item1.GetPropertyName();
				if (propsDict1.TryGetValue(name1, out var existingProp1))
					existingProp1.HasSetter = true;
				else
				{
					var prop1 = _makeProp(item1, isGetter: false, name1);
					prop1.HasSetter = true;
					propsDict1[name1] = prop1;
				}
			}

			var allProperties1 = propsDict1.Values;
			ReadWriteProperties = [.. allProperties1.Where(x => x.HasGetter && x.HasSetter)];
			ReadOnlyProperties = [.. allProperties1.Where(x => !x.HasSetter)];
			WriteOnlyProperties = [.. allProperties1.Where(x => !x.HasGetter)];

			// Парсинг функций
			Functions = [.. funcs1.Select(x => new ObjInfoFunction
			{
				Return = x.ReturnType.GetCSharpTypeName(),
				Name = x.Name,
				Generics = x.GetCSharpGenerics(),
				Parameters = x.GetCSharpParams()
			})];

			// Парсинг void-методов
			Methods = [.. meths1.Select(x => new ObjInfoMethod
			{
				Name = x.Name,
				Generics = x.GetCSharpGenerics(),
				Parameters = x.GetCSharpParams()
			})];
		}


		private static ObjInfoProperty _makeProp(
			MethodInfo info,
			bool isGetter,
			string? name = null)
		{
			var propertyType1 = isGetter
				? info.ReturnType
				: info.GetParameters()[0].ParameterType;
			return new ObjInfoProperty
			{
				Name = name ?? info.GetPropertyName(),
				TypeName = propertyType1.GetCSharpTypeName(),
				Value = null
			};
		}

	}

}
