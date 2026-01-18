using CsvHelper;
using System.Globalization;
using System.Text;

namespace Ans.Net10.Common
{

	public static class SuppCsv
	{

		/* functions */


		public static byte[] GetCsvBytesFromObject<T>(
			IEnumerable<T> items)
		{
			using var stream1 = new MemoryStream();
			using var writer1 = new StreamWriter(stream1, Encoding.UTF8);
			using var csv1 = new CsvWriter(writer1, CultureInfo.CurrentCulture);
			csv1.WriteRecords(items);
			writer1.Flush();
			stream1.Position = 0;
			return stream1.ToArray();
		}

	}

}
