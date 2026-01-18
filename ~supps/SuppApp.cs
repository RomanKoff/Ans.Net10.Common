using System.Reflection;

namespace Ans.Net10.Common
{

	public static partial class SuppApp
	{

		/* readonly properties */


		public static string CurrentPath
			=> Environment.CurrentDirectory;


		public static string DebugProjectPath
			=> field ??= Directory.GetParent(CurrentPath)
				.Parent.Parent.FullName;


		public static string DebugProjectNamespace
			=> field ??= DebugProjectPath.Split('/', '\\').Last();


		public static string DebugSolutionPath
			=> field ??= Directory.GetParent(CurrentPath)
				.Parent.Parent.Parent.FullName;


		public static string DebugSolutionNamespace
			=> field ??= DebugSolutionPath.Split('/', '\\').Last();


		/* functions */


		public static string GetName()
		{
			return Assembly.GetCallingAssembly()
				.GetName().Name;
		}


		public static string GetVersion()
		{
			var s1 = Assembly.GetCallingAssembly()
				.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
					.InformationalVersion.Split('+');
			return $"{s1[0]}";
		}


		public static string GetDescription()
		{
			return Assembly.GetCallingAssembly()
				.GetCustomAttribute<AssemblyDescriptionAttribute>()?
					.Description;
		}

	}

}