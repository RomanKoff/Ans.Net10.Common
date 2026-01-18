using Microsoft.Extensions.Configuration;

namespace Ans.Net10.Common
{

	public static partial class _e_IConfiguration
	{

		/* functions */


		public static string GetDatabaseName(
			this IConfiguration configuration,
			string connectionStringName)
		{
			var s1 = configuration.GetConnectionString(connectionStringName);
			var a1 = s1.Split(';');
			var name1 = a1.FirstOrDefault(x => x.Contains("Database="));
			return name1;
		}


		public static T GetOptions<T>(
			this IConfiguration configuration,
			string sectionName)
			where T : _AppSettingsOptions_Proto
		{
			var options1 = configuration.GetOptions<T>(sectionName)
				?? throw new Exception(
					$"[appsettings.json/{sectionName}] is required!");
			options1.SectionName = sectionName;
			options1.Test();
			return options1;
		}

	}

}
