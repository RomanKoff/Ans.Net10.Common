using System.Reflection;

namespace Ans.Net10.Common
{

	public class _ObjInfo_Base
	{
		public string Name { get; set; }
	}



	public class ObjInfoProperty
		: _ObjInfo_Base
	{
		public bool HasGetter { get; set; }
		public bool HasSetter { get; set; }
		public string TypeName { get; set; }
		public string Value { get; set; }
	}



	public class ObjInfoMethod
		: _ObjInfo_Base
	{
		public string Generics { get; set; }
		public IEnumerable<string> Parameters { get; set; }
	}



	public class ObjInfoFunction
		: ObjInfoMethod
	{
		public string Return { get; set; }
	}



	public class ObjInfoBuilder
	{

		/* ctor */


		public ObjInfoBuilder(
			Type type,
			object obj = null)
		{
			InfoType = type;
			_parse();
			if (obj != null)
			{
				foreach (var item1 in ReadWriteProperties)
					item1.Value = SuppReflection.GetCSharpValue(
						obj.GetPropertyValue(item1.Name));
				foreach (var item1 in ReadOnlyProperties)
					item1.Value = SuppReflection.GetCSharpValue(
						obj.GetPropertyValue(item1.Name));
			}
		}


		/* readonly properties */


		public Type InfoType { get; private set; }
		public IEnumerable<ObjInfoMethod> Ctors { get; private set; }
		public IEnumerable<ObjInfoProperty> ReadWriteProperties { get; private set; }
		public IEnumerable<ObjInfoProperty> ReadOnlyProperties { get; private set; }
		public IEnumerable<ObjInfoProperty> WriteOnlyProperties { get; private set; }
		public IEnumerable<ObjInfoFunction> Functions { get; private set; }
		public IEnumerable<ObjInfoMethod> Methods { get; private set; }

		public bool HasReadWriteProperties
			=> ReadWriteProperties?.Count() > 0;

		public bool HasReadonlyProperties
			=> ReadOnlyProperties?.Count() > 0;

		public bool HasWriteonlyProperties
			=> WriteOnlyProperties?.Count() > 0;

		public bool HasProperties
			=> HasReadWriteProperties || HasReadonlyProperties || HasWriteonlyProperties;


		/* privates */


		private void _parse()
		{
			var type0 = typeof(object);
			Ctors = InfoType.GetConstructors().Select(x => new ObjInfoMethod
			{
				Name = x.Name,
				Parameters = x.GetCSharpParams()
			});
			var methods1 = InfoType.GetMethods()
				.Where(x => x.DeclaringType != type0);
			//.OrderBy(x => x.Name);
			List<MethodInfo> getters1 = [];
			List<MethodInfo> setters1 = [];
			List<MethodInfo> funcs1 = [];
			List<MethodInfo> meths1 = [];
			foreach (var item1 in methods1)
				if (item1.Name.StartsWith("get_"))
					getters1.Add(item1);
				else if (item1.Name.StartsWith("set_"))
					setters1.Add(item1);
				else if (item1.ReturnType.Name == "Void")
					meths1.Add(item1);
				else
					funcs1.Add(item1);
			var props1 = new List<ObjInfoProperty>();
			foreach (var item1 in getters1)
			{
				var prop1 = _makeProp(item1);
				prop1.HasGetter = true;
				props1.Add(prop1);
			}
			foreach (var item1 in setters1)
			{
				var name1 = item1.GetPropertyName();
				var prop1 = props1.FirstOrDefault(x => x.Name == name1);
				if (prop1 != null)
					prop1.HasSetter = true;
				else
				{
					var prop2 = _makeProp(item1, name1);
					prop2.HasSetter = true;
					props1.Add(prop2);
				}
			}
			ReadWriteProperties = props1.Where(x => x.HasGetter && x.HasSetter);
			ReadOnlyProperties = props1.Where(x => !x.HasSetter);
			WriteOnlyProperties = props1.Where(x => !x.HasGetter);
			Functions = funcs1.Select(x => new ObjInfoFunction
			{
				Return = x.ReturnType.GetCSharpTypeName(),
				Name = x.Name,
				Generics = x.GetCSharpGenerics(),
				Parameters = x.GetCSharpParams()
			});
			Methods = meths1.Select(x => new ObjInfoMethod
			{
				Name = x.Name,
				Generics = x.GetCSharpGenerics(),
				Parameters = x.GetCSharpParams()
			});
		}


		private static ObjInfoProperty _makeProp(
			MethodInfo info,
			string name = null)
		{
			var prop1 = new ObjInfoProperty
			{
				Name = name ?? info.GetPropertyName(),
				TypeName = info.ReturnType.GetCSharpTypeName(),
				Value = SuppReflection.GetCSharpValue(info),
			};
			return prop1;
		}

	}

}
