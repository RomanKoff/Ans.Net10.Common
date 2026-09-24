// rev 2026-09-22

using CsvHelper;
using System.Globalization;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для высокопроизводительной работы с данными в формате CSV.
	/// </summary>
	public static class SuppCsv
	{

		/* functions */

		/// <summary>
		/// Сериализует коллекцию объектов в массив байт CSV (UTF-8 без BOM).
		/// </summary>
		/// <typeparam name="T">Тип сериализуемых объектов.</typeparam>
		/// <param name="items">Коллекция элементов для выгрузки в CSV.</param>
		/// <returns>Массив байт, представляющий документ в формате CSV.</returns>
		public static byte[] GetCsvBytesFromObject<T>(
			IEnumerable<T> items)
		{
			using var stream1 = new MemoryStream();
			// Использование UTF-8 без BOM — лучшая практика для веб-интерфейсов и API
			using var writer1 = new StreamWriter(stream1, new UTF8Encoding(false));
			using var csv1 = new CsvWriter(writer1, CultureInfo.CurrentCulture);
			csv1.WriteRecords(items);
			writer1.Flush();
			return stream1.ToArray();
		}

	}


}
