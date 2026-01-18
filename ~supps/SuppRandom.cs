using System.Security.Cryptography;
using System.Text;

namespace Ans.Net10.Common
{

	public static class SuppRandom
	{

		/* functions */


		public static int Next()
		{
			using var gen1 = RandomNumberGenerator.Create();
			var a1 = new byte[4];
			gen1.GetBytes(a1);
			return Math.Abs(BitConverter.ToInt32(a1, 0));
		}


		public static int Next(
			int max)
		{
			return Next() % (max + 1);
		}


		public static int Next(
			int min,
			int max)
		{
			return Next(max - min) + min;
		}


		public static string Generate(
			string mask,
			int minLength,
			int maxLength)
		{
			var a1 = mask.ToCharArray();
			int l1 = (minLength == maxLength)
				? minLength : Next(minLength, maxLength);
			var sb1 = new StringBuilder(l1);
			for (int i1 = 0; i1 < l1; i1++)
				sb1.Append(a1[Next(a1.Length - 1)]);
			return sb1.ToString();
		}


		public static string Generate(
			string mask,
			int length)
		{
			return Generate(mask, length, length);
		}

	}

}
