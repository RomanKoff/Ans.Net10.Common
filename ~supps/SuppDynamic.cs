// rev 2026-09-

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/*
	 * For Razor use ViewData.Get...
	 */



	/// <summary>
	/// Вспомогательный класс для работы с динамическими объектами (dynamic).
	/// </summary>
	public static class SuppDynamic
	{

		/* functions */


		/// <summary>
		/// Проверяет наличие свойства с указанным именем у динамического объекта.
		/// </summary>
		/// <param name="item">Динамический объект для проверки.</param>
		/// <param name="propertyName">Имя искомого свойства.</param>
		/// <returns><see langword="true"/>, если свойство найдено; иначе — <see langword="false"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasProperty(
			dynamic item,
			string propertyName)
		{
			if (item is IDictionary<string, object> dict1)
				return dict1.ContainsKey(propertyName);
			return item.GetType().GetProperty(propertyName) != null;
		}

	}

}
