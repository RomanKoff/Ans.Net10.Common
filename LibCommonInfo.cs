// rev 2026-09-10

namespace Ans.Net10.Common
{

	public static class LibCommonInfo
	{
		public static string Name => SuppApp.CallingAssembly.Name;
		public static string Version => SuppApp.CallingAssembly.Version;
		public static string FullVersion => SuppApp.CallingAssembly.FullVersion;
		public static string? Description => SuppApp.CallingAssembly.Description;
	}

}
