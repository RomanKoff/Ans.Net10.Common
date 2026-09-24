// rev 2026-09-16

using System.Buffers;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для оптимизированной работы с объектами <see cref="StringBuilder"/>.
	/// </summary>
	public static class SuppStringBuilder
	{

		private static readonly SearchValues<char> _invalidChars = SearchValues.Create(
			"\0\x01\x02\x03\x04\x05\x06\a\b\t\n\v\f\r\x0e\x0f\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1a\x1b\x1c\x1d\x1e\x1f");


		/* methods */


		/// <summary>
		/// Заменяет все управляющие спецсимволы (с ASCII-кодом меньше 32)
		/// внутри <see cref="StringBuilder"/> на обычные пробелы.
		/// </summary>
		/// <param name="sb">Модифицируемый экземпляр StringBuilder.</param>
		public static void FixSpecChars(
			StringBuilder sb)
		{
			ArgumentNullException.ThrowIfNull(sb);
			if (sb.Length == 0)
				return;
			bool hasInvalid1 = false;
			foreach (var chunk1 in sb.GetChunks())
				if (chunk1.Span.IndexOfAny(_invalidChars) != -1)
				{
					hasInvalid1 = true;
					break;
				}
			if (!hasInvalid1)
				return;
			int len1 = sb.Length;
			var rented1 = ArrayPool<char>.Shared.Rent(len1);
			try
			{
				sb.CopyTo(0, rented1, 0, len1);
				var span1 = rented1.AsSpan(0, len1);
				int i1;
				while ((i1 = span1.IndexOfAny(_invalidChars)) != -1)
					span1[i1] = ' ';
				sb.Clear();
				sb.Append(span1);
			}
			finally
			{
				ArrayPool<char>.Shared.Return(rented1);
			}
		}

	}

}
