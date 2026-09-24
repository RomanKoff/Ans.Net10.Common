// rev 2026-09-22

using System.Globalization;
using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Предоставляет методы безопасного и типизированного извлечения данных 
	/// из отдельной записи (строки) формата GRID.
	/// </summary>
	public sealed class GridRowParser
	{

		private readonly string[] _fields;
		private readonly IFormatProvider _provider;


		/* ctor */


		/// <summary>
		/// Инициализирует новый экземпляр парсера строки GRID на основе массива сырых полей.
		/// </summary>
		/// <param name="fields">Массив строковых полей, извлеченных из записи.</param>
		/// <param name="provider">
		/// Поставщик форматирования для парсинга чисел и дат.
		/// Если null — используется <see cref="CultureInfo.InvariantCulture"/>.
		/// </param>
		public GridRowParser(
			string[] fields,
			IFormatProvider? provider)
		{
			_fields = fields;
			_provider = provider ?? CultureInfo.InvariantCulture;
		}


		/* functions */


		/// <summary>
		/// Безопасно возвращает строковое значение по указанному индексу
		/// с автоматическим удалением пробелов.
		/// </summary>
		/// <param name="index">Индекс поля (начиная с 0).</param>
		/// <returns>
		/// Очищенная строка ячейки. Если индекс вне границ
		/// или поле пусто — возвращает <see cref="string.Empty"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public string GetText(
			int index)
		{
			if (index < 0 || index >= _fields.Length)
				return string.Empty;

			string val1 = _fields[index];
			return string.IsNullOrEmpty(val1)
				? string.Empty
				: val1.Trim();
		}


		/// <summary>
		/// Возвращает целочисленное значение по указанному индексу
		/// с использованием заданного провайдера формата.
		/// </summary>
		/// <param name="index">Индекс поля (начиная с 0).</param>
		/// <returns>
		/// Целое число. Если индекс вне границ,
		/// поле пусто или конвертация не удалась — возвращает 0.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetInt(
			int index)
		{
			string val1 = GetText(index);
			return string.IsNullOrEmpty(val1)
				? 0
				: val1.ToInt(0, _provider);
		}


		/// <summary>
		/// Возвращает вещественное значение типа double по указанному индексу
		/// с использованием заданного провайдера формата.
		/// </summary>
		/// <param name="index">Индекс поля (начиная с 0).</param>
		/// <returns>
		/// Число с плавающей запятой. Если индекс вне границ,
		/// поле пусто или конвертация не удалась — возвращает 0.0.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double GetReal(
			int index)
		{
			string val1 = GetText(index);
			return string.IsNullOrEmpty(val1)
				? 0.0
				: val1.ToDouble(0.0, _provider);
		}


		/// <summary>
		/// Возвращает значение даты и времени по указанному индексу
		/// с использованием заданного провайдера формата.
		/// </summary>
		/// <param name="index">Индекс поля (начиная с 0).</param>
		/// <returns>
		/// Объект DateTime или null, если значение отсутствует,
		/// некорректно или индекс вне границ.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DateTime? GetDate(
			int index)
		{
			string val1 = GetText(index);
			return string.IsNullOrEmpty(val1)
				? null
				: val1.ToDateTime(_provider);
		}


		/// <summary>
		/// Возвращает логическое значение по указанному индексу.
		/// </summary>
		/// <param name="index">Индекс поля (начиная с 0).</param>
		/// <returns>
		/// Значение true, если строка равна "1" или "true" (без учета регистра); иначе false.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool GetBool(
			int index)
		{
			string val1 = GetText(index);
			return !string.IsNullOrEmpty(val1) && val1.ToBool();
		}

	}

}
