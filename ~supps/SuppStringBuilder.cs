using System.Buffers;
using System.Text;

namespace Ans.Net10.Common
{

	public static class SuppStringBuilder
	{

		private static readonly SearchValues<char> _invalidChars = SearchValues.Create(
			"\0\x01\x02\x03\x04\x05\x06\a\b\t\n\v\f\r\x0e\x0f\x10\x11\x12\x13\x14\x15\x16\x17\x18\x19\x1a\x1b\x1c\x1d\x1e\x1f");


		/* methods */


		public static void FixSpecChars(
			StringBuilder sb)
		{
			int length1 = sb.Length;
			if (length1 == 0)
				return;

			bool f1 = false;
			foreach (var chunk1 in sb.GetChunks())
				if (chunk1.Span.IndexOfAny(_invalidChars) != -1)
				{
					f1 = true;
					break;
				}
			if (!f1)
				return;

			var buffer1 = ArrayPool<char>.Shared.Rent(length1);
			try
			{
				sb.CopyTo(0, buffer1, 0, length1);
				for (int i1 = 0; i1 < length1; i1++)
					if (buffer1[i1] < 32)
						sb[i1] = ' ';									
			}
			finally
			{
				ArrayPool<char>.Shared.Return(buffer1);
			}
		}

	}

}
