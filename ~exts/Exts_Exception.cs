// rev 2026-09-18

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class Exts_Exception
	{

		/* functions */


		/// <summary>
		/// Возвращает текстовое сообщение самого глубокого (первичного) исключения в цепочке InnerException.
		/// </summary>
		/// <param name="exception">Исходное исключение.</param>
		/// <returns>Текст сообщения об ошибке первичного исключения.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetExceptionMessage(
			this Exception exception)
		{
			return (exception.InnerException == null)
				? exception.Message
				: exception.InnerException.GetExceptionMessage();
		}


		/// <summary>
		/// Рекурсивно проверяет, содержит ли сообщение текущего
		/// или любого вложенного исключения указанную подстроку.
		/// Сравнение производится без учета регистра символов.
		/// </summary>
		/// <param name="exception">Исходное исключение для проверки.</param>
		/// <param name="value">Искомая подстрока.</param>
		/// <returns><see langword="true"/>, если подстрока найдена в цепочке исключений; иначе — <see langword="false"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TestContains(
			this Exception exception,
			string value)
		{
			if (string.IsNullOrEmpty(value))
				return false;
			if (exception.Message.Contains(value, StringComparison.OrdinalIgnoreCase))
				return true;
			return exception.InnerException != null
				&& exception.InnerException.TestContains(value);
		}


		/// <summary>
		/// Рекурсивно проверяет, начинается ли сообщение текущего
		/// или любого вложенного исключения с указанной подстроки.
		/// Сравнение производится без учета регистра символов.
		/// </summary>
		/// <param name="exception">Исходное исключение для проверки.</param>
		/// <param name="value">Искомая подстрока начала сообщения.</param>
		/// <returns><see langword="true"/>, если сообщение в цепочке начинается с указанной подстроки; иначе — <see langword="false"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TestStartsWith(
			this Exception exception,
			string value)
		{
			if (string.IsNullOrEmpty(value))
				return false;
			if (exception.Message.StartsWith(value, StringComparison.OrdinalIgnoreCase))
				return true;
			return exception.InnerException != null
				&& exception.InnerException.TestStartsWith(value);
		}

	}

}
