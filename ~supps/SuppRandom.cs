// rev 2026-09-11

using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для генерации
	/// криптографически стойких случайных данных.
	/// </summary>
	public static class SuppRandom
	{

		/// <summary>
		/// Возвращает криптографически стойкое случайное число в диапазоне от min до max включительно.
		/// </summary>
		/// <remarks>
		/// Автоматически переворачивает диапазон, если <paramref name="min"/> &gt; <paramref name="max"/>.
		/// </remarks>
		/// <param name="min">Нижняя граница диапазона.</param>
		/// <param name="max">Верхняя граница диапазона.</param>
		/// <returns>Случайное целое число внутри заданного диапазона включительно.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetInt(
			int min,
			int max)
		{
			if (min > max)
				(min, max) = (max, min);
			if (max < int.MaxValue)
				return RandomNumberGenerator.GetInt32(min, max + 1);
			if (min > int.MinValue)
				return RandomNumberGenerator.GetInt32(min - 1, int.MaxValue) + 1;
			Span<byte> bytes = stackalloc byte[4];
			RandomNumberGenerator.Fill(bytes);
			return BitConverter.ToInt32(bytes);
		}


		/// <summary>
		/// Генерирует случайную строку на основе маски символов и диапазона длин.
		/// </summary>
		/// <param name="mask">Строка-маска, содержащая набор доступных символов.</param>
		/// <param name="minLength">Минимально возможная длина строки.</param>
		/// <param name="maxLength">Максимально возможная длина строки.</param>
		/// <returns>Случайная строка заданной длины, состоящая из символов маски.</returns>
		/// <exception cref="ArgumentException">Выбрасывается, если маска пустая или равна <see langword="null"/>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если вычисленная минимальная длина меньше 0.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetString(
			string mask,
			int minLength,
			int maxLength)
		{
			ArgumentException.ThrowIfNullOrEmpty(
				mask, nameof(mask));
			if (minLength > maxLength)
				(minLength, maxLength) = (maxLength, minLength);
			ArgumentOutOfRangeException.ThrowIfLessThan(
				minLength, 0, nameof(minLength));
			int length1 = GetInt(minLength, maxLength);
			return length1 == 0
				? string.Empty
				: RandomNumberGenerator.GetString(mask, length1);
		}

	}

}
