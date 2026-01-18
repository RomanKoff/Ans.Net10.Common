using Microsoft.Extensions.DependencyInjection;

namespace Ans.Net10.Common
{

	public static partial class __e_di
	{

		public static bool HasService(
			this IServiceCollection services,
			Type type)
		{
			return services.Any(x => x.ServiceType == type);
		}

	}

}
