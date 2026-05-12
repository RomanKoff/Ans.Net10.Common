using System.Security.Cryptography;
using System.Text;

namespace Ans.Net10.Common
{

	public static class SuppCrypto
	{

		/* functions */


		public static byte[] GetHash(
			string value,
			byte[] salt)
		{
			using var alg1 = new HMACSHA1(salt);
			try
			{
				var a1 = Encoding.UTF8.GetBytes(value);
				return alg1.ComputeHash(a1);
			}
			finally { alg1.Clear(); }
		}


		public static string GetHashString(
			string value,
			byte[] salt)
		{
			var hash1 = GetHash(value, salt);
			return string.Join(
				"", hash1.Select(x => x.ToString("x2")));
		}


		public static string GetHashString(
			string value,
			string salt)
		{
			var a1 = Encoding.Unicode.GetBytes(salt);
			return GetHashString(value, a1);
		}

	}

}
