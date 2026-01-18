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
				return alg1.ComputeHash(Encoding.UTF8.GetBytes(value));
			}
			finally { alg1.Clear(); }
		}


		public static string GetHashString(
			string value,
			byte[] salt)
		{
			var hash1 = GetHash(value, salt);
			return string.Join(
				"", hash1.Select(x => x.ToString("x2"))); // Convert.ToBase64String(result);
		}


		public static string GetHashString(
			string value,
			string salt)
		{
			return GetHashString(value, Encoding.Unicode.GetBytes(salt));
		}

	}

}
