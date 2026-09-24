// rev 2026-09-16

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class SuppLangRu
	{

		/// <summary>
		/// Возвращает случайную полноценную текстовую панграмму на русском языке.
		/// </summary>
		/// <returns>Строка, содержащая полный текстовый семпл.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetSample()
			=> _Consts.GetRandomSampleRu();


		/// <summary>
		/// Возвращает случайную укороченную фразу (средний семпл) на русском языке.
		/// </summary>
		/// <returns>Строка, содержащая средний текстовый семпл.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetSampleSmall()
			=> _Consts.GetRandomSampleSmallRu();


		/// <summary>
		/// Возвращает случайное короткое словосочетание (минимальный семпл) на русском языке.
		/// </summary>
		/// <returns>Строка, содержащая минимальный текстовый семпл.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetSampleSmaller()
			=> _Consts.GetRandomSampleSmallerRu();

	}

}
