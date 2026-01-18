namespace Ans.Net10.Common
{

	public static partial class __e_io
	{

		public static IEnumerable<FileInfo> GetFilesByExtensions(
			this DirectoryInfo directoryInfo,
			params string[] extensions)
		{
			ArgumentNullException.ThrowIfNull(extensions);
			var files1 = directoryInfo.EnumerateFiles();
			return files1.Where(x => extensions.Contains(x.Extension));
		}


		public static IEnumerable<FileInfo> GetFilesByExtensions(
			this DirectoryInfo directoryInfo,
			string extensions)
		{
			return GetFilesByExtensions(
				directoryInfo,
				extensions.Split(_Consts.SEPS_ARRAY));
		}

	}

}
