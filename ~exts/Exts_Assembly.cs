// rev 2026-09-10

using System.Reflection;

namespace Ans.Net10.Common
{

	public static partial class Exts_Assembly
	{

		extension(Assembly? instance)
		{

			/* readonly properties */


			/// <summary>
			/// Возвращает имя сборки.
			/// </summary>
			/// <value>Имя сборки. Если сборка не задана, возвращает "UnknownAssembly".</value>
			public string Name
				=> instance switch
				{
					null => "UnknownAssembly",
					_ => instance.GetName().Name ?? instance.ToString()
				};


			/// <summary>
			/// Возвращает версию сборки без учета хеша коммита.
			/// </summary>
			/// <value>Строка версии. Если сборка не задана или версию определить не удалось, возвращает "1.0.0".</value>
			public string Version
			{
				get
				{
					if (instance is null)
						return "1.0.0";
					if (instance.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
						is { InformationalVersion: { } info1 })
					{
						var span1 = info1.AsSpan();
						var i1 = span1.IndexOf('+');
						return i1 == -1
							? info1 : span1[..i1].ToString();
					}
					return instance.GetName().Version?.ToString() ?? "1.0.0";
				}
			}


			/// <summary>
			/// Возвращает полную версию сборки, включая хеш коммита Git (при его наличии).
			/// </summary>
			/// <value>Полная строка информационной версии. Если сборка не задана, возвращает "1.0.0".</value>
			public string FullVersion
			{
				get
				{
					if (instance is null)
						return "1.0.0";
					return instance.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
						?? instance.GetName().Version?.ToString()
						?? "1.0.0";
				}
			}


			/// <summary>
			/// Возвращает описание сборки из метаданных.
			/// </summary>
			/// <value>Текст описания сборки или <see langword="null"/>, если описание отсутствует.</value>
			public string? Description
				=> instance?.GetCustomAttribute<AssemblyDescriptionAttribute>()
					is { Description: { Length: > 0 } desc1 }
					? desc1
					: null;

		}

	}

}
