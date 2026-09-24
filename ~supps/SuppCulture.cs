// rev 2026-09-16

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для управления региональными настройками (культурой) приложения.
	/// </summary>
	public static class SuppCulture
	{

		/* methods */


		/// <summary>
		/// Добавляет регистрацию расширенных кодовых страниц для .NET 10
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AddCodePagesSupport()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}


		/// <summary>
		/// Устанавливает заданную культуру для текущего потока и его асинхронного контекста.
		/// </summary>
		/// <param name="culture">Объект информации о культуре.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetCulture(
			CultureInfo culture)
		{
			CultureInfo.CurrentUICulture = culture;
			CultureInfo.CurrentCulture = culture;
		}


		/// <summary>
		/// Устанавливает культуру по её строковому имени (например, "ru-RU" или "en-US").
		/// </summary>
		/// <param name="culture">Строковое имя культуры.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetCulture(
			string culture)
		{
			SetCulture(new CultureInfo(culture));
		}

	}

}
