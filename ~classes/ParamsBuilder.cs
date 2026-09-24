// rev 2026-09-19

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Строитель параметров URL-запроса, поддерживающий безопасное
	/// динамическое добавление временных параметров.
	/// </summary>
	public class ParamsBuilder
	{

		/* readonly properties */


		/// <summary>
		/// Возвращает базовую коллекцию параметров.
		/// </summary>
		public ParamsCollection Parameters { get; } = new();


		/* methods */


		/// <summary>
		/// Добавляет или обновляет строковый параметр.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			string value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет логический параметр.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			bool value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет целочисленный параметр.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			int value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет числовой параметр типа long.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			long value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет числовой параметр типа double.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			double value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет числовой параметр типа float.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			float value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет числовой параметр типа decimal.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			decimal value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет параметр даты и времени.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			DateTime? value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет параметр даты.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			DateOnly? value)
		{
			Parameters.Append(name, value);
		}


		/// <summary>
		/// Добавляет параметр времени.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(
			string name,
			TimeOnly? value)
		{
			Parameters.Append(name, value);
		}


		/* functions */


		/// <summary>
		/// Генерирует строку запроса, объединяя текущую коллекцию с одним
		/// дополнительным строковым параметром без изменения основного состояния.
		/// </summary>
		/// <param name="additionName">Имя временного параметра.</param>
		/// <param name="additionValue">Значение временного параметра.</param>
		/// <returns>Итоговая строка URL-запроса.</returns>
		public string GetString(
			string additionName,
			string additionValue)
		{
			var tempCollection1 = new ParamsCollection(
				Parameters.Items.Count + 1, Parameters.Items.Comparer);
			foreach (var item1 in Parameters.Items)
				tempCollection1.Append(item1.Key, item1.Value);
			tempCollection1.Append(additionName, additionValue);
			return tempCollection1.ToString();
		}


		/// <summary>
		/// Генерирует строку запроса, объединяя текущую коллекцию с одним
		/// дополнительным целочисленным параметром без изменения основного состояния.
		/// </summary>
		/// <param name="additionName">Имя временного параметра.</param>
		/// <param name="additionValue">Числовое значение временного параметра.</param>
		/// <returns>Итоговая строка URL-запроса.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetString(
			string additionName,
			int additionValue)
		{
			return GetString(
				additionName, additionValue.ToString());
		}


		/// <summary>
		/// Возвращает строку запроса на основе текущего состояния строителя.
		/// </summary>
		public override string ToString()
		{
			return Parameters.ToString();
		}

	}

}
