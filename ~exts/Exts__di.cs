// rev 2026-09-17

using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class Exts__di
	{

		/// <summary>
		/// Проверяет, зарегистрирована ли служба указанного типа в коллекции дескрипторов служб.
		/// </summary>
		/// <param name="services">Коллекция дескрипторов служб.</param>
		/// <param name="type">Тип проверяемой службы.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasService(
			this IServiceCollection services,
			Type type)
		{
			return services.Any(
				x => x.ServiceType == type);
		}

	}

}
