// rev 2026-09-10

using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Предоставляет криптографические методы для
	/// работы с паролями, токенами, хэшами (SHA-256) и HMAC.
	/// </summary>
	public static class SuppCrypto
	{

		private const int _SALT_SIZE = 16; // 128 бит соль
		private const int _KEY_SIZE = 32;  // 256 бит под хэш
		private const int _ITERATIONS = 600_000; // Рекомендация OWASP / NIST для PBKDF2-SHA256

		private static readonly char[] _LOWERCASE = "abcdefghjkmnpqrstuvwxyz".ToCharArray();
		private static readonly char[] _UPPERCASE = "ABCDEFGHJKMNPQRSTUVWXYZ".ToCharArray();
		private static readonly char[] _DIGITS = "23456789".ToCharArray();
		private static readonly char[] _SPECIALCHARS = "!@#$%^&*()_+-=[]{}?<>-".ToCharArray();
		private static readonly char[] _ALL_CHARS = "abcdefghjkmnpqrstuvwxyzABCDEFGHJKMNPQRSTUVWXYZ23456789!@#$%^&*()_+-=[]{}?<>-".ToCharArray();


		/// <summary>
		/// Генерирует криптографически стойкий случайный пароль заданной длины.
		/// Гарантирует наличие как минимум одного символа из каждой группы (нижний регистр, верхний регистр, цифры, знаки).
		/// </summary>
		/// <param name="length">Длина генерируемого пароля (не менее 8 символов).</param>
		/// <returns>Строка безопасного случайного пароля.</returns>
		/// <exception cref="ArgumentException">Выбрасывается, если длина пароля меньше 8 символов.</exception>
		public static string GenerateSecurePassword(
			int length = 16)
		{
			if (length < 8)
				throw new ArgumentException(
					"[Ans.Net10.Common/SuppCrypto.GenerateSecurePassword] To ensure security, the password must be at least 8 characters long.",
					nameof(length));
			var a1 = new char[length];
			a1[0] = _LOWERCASE[RandomNumberGenerator.GetInt32(_LOWERCASE.Length)];
			a1[1] = _UPPERCASE[RandomNumberGenerator.GetInt32(_UPPERCASE.Length)];
			a1[2] = _DIGITS[RandomNumberGenerator.GetInt32(_DIGITS.Length)];
			a1[3] = _SPECIALCHARS[RandomNumberGenerator.GetInt32(_SPECIALCHARS.Length)];
			for (int i1 = 4; i1 < length; i1++)
				a1[i1] = _ALL_CHARS[RandomNumberGenerator.GetInt32(_ALL_CHARS.Length)];
			for (int i1 = length - 1; i1 > 0; i1--)
			{
				int j1 = RandomNumberGenerator.GetInt32(i1 + 1);
				(a1[i1], a1[j1]) = (a1[j1], a1[i1]);
			}
			return new string(a1);
		}


		/// <summary>
		/// Генерирует криптографически стойкий URL-безопасный токен
		/// в формате Base64Url без выделения промежуточных строк.
		/// </summary>
		/// <param name="bytesCount">Количество байт энтропии (например, 16 байт = 128 бит, 32 байта = 256 бит безопасности).</param>
		/// <returns>Строка токена в формате Base64Url. Если количество байт меньше или равно 0, возвращается пустая строка.</returns>
		public static string GenerateApiToken(
			int bytesCount = 32)
		{
			if (bytesCount <= 0)
				return string.Empty;
			Span<byte> buffer1 = bytesCount <= 256
				? stackalloc byte[bytesCount]
				: new byte[bytesCount];
			RandomNumberGenerator.Fill(buffer1);
			return Convert.ToBase64String(buffer1)
				.Replace("+", "-")
				.Replace("/", "_")
				.TrimEnd('=');
		}


		/// <summary>
		/// Создает безопасный хэш пароля на основе PBKDF2-SHA256 с добавлением соли.
		/// </summary>
		/// <param name="password">Исходный пароль для хэширования.</param>
		/// <returns>Строка хэша, содержащая метаданные и соль в формате Base64, готовую к сохранению.</returns>
		/// <exception cref="ArgumentException">Выбрасывается, если пароль пустой или равен null.</exception>
		public static string HashPassword(
			string password)
		{
			ArgumentException.ThrowIfNullOrEmpty(
				password, nameof(password));
			var salt1 = RandomNumberGenerator.GetBytes(_SALT_SIZE);
			var hash1 = Rfc2898DeriveBytes.Pbkdf2(
				password, salt1, _ITERATIONS, HashAlgorithmName.SHA256, _KEY_SIZE);
			return $"{Convert.ToBase64String(salt1)}.{Convert.ToBase64String(hash1)}";
		}


		/// <summary>
		/// Проверяет соответствие открытого пароля сохраненному хэшу. Защищено от атак по времени (Timing Attacks).
		/// </summary>
		/// <param name="password">Проверяемый открытый пароль.</param>
		/// <param name="hashedPassword">Ранее сохраненная строка хэша в формате "Соль.Хэш".</param>
		/// <returns><see langword="true"/>, если пароль верен; иначе — <see langword="false"/>.</returns>
		/// <exception cref="ArgumentException">Выбрасывается при пустых параметрах.</exception>
		public static bool VerifyPassword(
			string password,
			string hashedPassword)
		{
			ArgumentException.ThrowIfNullOrEmpty(
				password, nameof(password));
			ArgumentException.ThrowIfNullOrEmpty(
				hashedPassword, nameof(hashedPassword));
			var i1 = hashedPassword.IndexOf('.');
			if (i1 == -1 || i1 == 0 || i1 == hashedPassword.Length - 1)
				return false;
			try
			{
				var span1 = hashedPassword.AsSpan();
				var salt1 = Convert.FromBase64String(hashedPassword[..i1]);
				var expectedHash1 = Convert.FromBase64String(hashedPassword[(i1 + 1)..]);
				var actualHash1 = Rfc2898DeriveBytes.Pbkdf2(
					password, salt1, _ITERATIONS, HashAlgorithmName.SHA256, _KEY_SIZE);
				return CryptographicOperations.FixedTimeEquals(actualHash1, expectedHash1);
			}
			catch (FormatException)
			{
				return false;
			}
		}


		/// <summary>
		/// Вычисляет хэш SHA-256 для строки и возвращает его в виде шестнадцатеричной строки в нижнем регистре.
		/// </summary>
		/// <param name="data">Исходная строка данных.</param>
		/// <returns>Строка хэша в нижнем регистре. Если входная строка null, возвращается пустая строка.</returns>
		public static string ComputeSha256(
			string? data)
		{
			if (data == null)
				return string.Empty;
			return Convert.ToHexStringLower(
				SHA256.HashData(Encoding.UTF8.GetBytes(data)));
		}


		/// <summary>
		/// Вычисляет хэш SHA-256 для байтового среза. Zero-allocation.
		/// </summary>
		/// <param name="source">Исходный байтовый срез.</param>
		/// <returns>Массив байт хэша.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] ComputeSha256(
			ReadOnlySpan<byte> source)
		{
			return SHA256.HashData(source);
		}


		/// <summary>
		/// Потокобезопасно и буферизованно вычисляет хэш SHA-256 для переданного потока данных.
		/// </summary>
		/// <param name="stream">Исходный поток данных.</param>
		/// <returns>Массив байт хэша. Если поток null, возвращается пустой массив.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] ComputeSha256(
			Stream? stream)
		{
			if (stream == null)
				return [];
			return SHA256.HashData(stream);
		}


		/// <summary>
		/// Вычисляет HMAC-SHA256 для строки с использованием секретного ключа.
		/// </summary>
		/// <param name="data">Исходная строка данных.</param>
		/// <param name="key">Секретный ключ.</param>
		/// <returns>Строка хэша в нижнем регистре. Если данные или ключ null, возвращается пустая строка.</returns>
		public static string ComputeHmacSha256(
			string? data,
			byte[]? key)
		{
			if (data == null || key == null)
				return string.Empty;
			var data1 = Encoding.UTF8.GetBytes(data);
			var hash1 = HMACSHA256.HashData(key, data1);
			return Convert.ToHexStringLower(hash1);
		}


		/// <summary>
		/// Вычисляет HMAC-SHA256 для байтового среза без создания объектов алгоритма в куче.
		/// </summary>
		/// <param name="source">Исходный байтовый срез.</param>
		/// <param name="key">Секретный ключ в виде среза.</param>
		/// <returns>Массив байт хэша.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] ComputeHmacSha256(
			ReadOnlySpan<byte> source,
			ReadOnlySpan<byte> key)
		{
			return HMACSHA256.HashData(key, source);
		}


		/// <summary>
		/// Вычисляет HMAC-SHA256 для потока данных с использованием секретного ключа.
		/// </summary>
		/// <param name="stream">Исходный поток данных.</param>
		/// <param name="key">Секретный ключ.</param>
		/// <returns>Массив байт хэша. Если поток или ключ null, возвращается пустой массив.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] ComputeHmacSha256(
			Stream? stream,
			byte[]? key)
		{
			if (stream == null || key == null)
				return [];
			return HMACSHA256.HashData(key, stream);
		}


		/// <summary>
		/// Преобразует строку в безопасный массив байт (UTF-8).
		/// </summary>
		/// <param name="source">Исходная строка.</param>
		/// <returns>Массив байт UTF-8. Если строка null, возвращается пустой массив.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] ToSecureBytes(
			string? source)
		{
			if (source == null)
				return [];
			return Encoding.UTF8.GetBytes(source);
		}


		/// <summary>
		/// Восстанавливает строку из массива байт (UTF-8).
		/// </summary>
		/// <param name="source">Исходный массив байт.</param>
		/// <returns>Декодированная строка. Если массив null, возвращается пустая строка.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ToSecureString(
			byte[]? source)
		{
			if (source == null)
				return string.Empty;
			return Encoding.UTF8.GetString(source);
		}


	}

}
