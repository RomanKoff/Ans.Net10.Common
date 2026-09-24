// rev 2026-09-18

using System.Runtime.CompilerServices;
using System.Text;

namespace Ans.Net10.Common
{

	public static partial class Exts_StringBuilder
	{

		/* methods */


		/// <summary>
		/// Добавляет отформатированную строку в конец, если строка шаблона не является пустой.
		/// </summary>
		/// <param name="sb">Экземпляр StringBuilder.</param>
		/// <param name="template">Шаблон строки форматирования.</param>
		/// <param name="templateArgs">Аргументы форматирования.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AppendIfPresent(
			this StringBuilder sb,
			string? template,
			params object[] templateArgs)
		{
			if (string.IsNullOrEmpty(template))
				return;
			if (templateArgs == null || templateArgs.Length == 0)
				sb.Append(template);
			else
				sb.AppendFormat(template, templateArgs);
		}


		/// <summary>
		/// Вставляет отформатированную строку по указанному индексу.
		/// </summary>
		/// <param name="sb">Экземпляр StringBuilder.</param>
		/// <param name="index">Индекс для вставки.</param>
		/// <param name="template">Шаблон строки форматирования.</param>
		/// <param name="templateArgs">Аргументы форматирования.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InsertFormat(
			this StringBuilder sb,
			int index,
			string template,
			params object[] templateArgs)
		{
			if (templateArgs == null || templateArgs.Length == 0)
				sb.Insert(index, template);
			else
				sb.Insert(index, string.Format(template, templateArgs));
		}


		/// <summary>
		/// Вставляет отформатированную строку по указанному индексу, если строка шаблона не является пустой.
		/// </summary>
		/// <param name="sb">Экземпляр StringBuilder.</param>
		/// <param name="index">Индекс для вставки.</param>
		/// <param name="template">Шаблон строки форматирования.</param>
		/// <param name="templateArgs">Аргументы форматирования.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InsertIfPresent(
			this StringBuilder sb,
			int index,
			string? template,
			params object[] templateArgs)
		{
			if (!string.IsNullOrEmpty(template))
				sb.InsertFormat(index, template, templateArgs);
		}


		/// <summary>
		/// Вставляет отформатированную строку по указанному индексу, если логическое условие истинно.
		/// </summary>
		/// <param name="sb">Экземпляр StringBuilder.</param>
		/// <param name="expression">Логическое условие выполнения вставки.</param>
		/// <param name="index">Индекс для вставки.</param>
		/// <param name="template">Шаблон строки форматирования.</param>
		/// <param name="templateArgs">Аргументы форматирования.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void InsertIf(
			this StringBuilder sb,
			bool expression,
			int index,
			string template,
			params object[] templateArgs)
		{
			if (expression)
				sb.InsertFormat(index, template, templateArgs);
		}


		/* functions */


		/// <summary>
		/// Возвращает индекс первого вхождения указанной строки в StringBuilder начиная с заданной позиции.
		/// </summary>
		/// <param name="sb">Экземпляр StringBuilder для поиска.</param>
		/// <param name="value">Искомая подстрока.</param>
		/// <param name="startIndex">Начальная позиция поиска.</param>
		/// <param name="ignoreCase">Признак игнорирования регистра символов (используется инвариантная культура).</param>
		/// <returns>Индекс начала вхождения подстроки или -1, если подстрока не найдена.</returns>
		public static int IndexOf(
			this StringBuilder sb,
			string value,
			int startIndex,
			bool ignoreCase)
		{
			if (string.IsNullOrEmpty(value))
				return -1;
			int valueLength1 = value.Length;
			int maxIndex1 = sb.Length - valueLength1;
			if (startIndex < 0 || startIndex > maxIndex1)
				return -1;
			if (!ignoreCase)
			{
				for (int i1 = startIndex; i1 <= maxIndex1; i1++)
				{
					bool match1 = true;
					for (int j1 = 0; j1 < valueLength1; j1++)
						if (sb[i1 + j1] != value[j1])
						{
							match1 = false;
							break;
						}
					if (match1)
						return i1;
				}
			}
			else
			{
				for (int i1 = startIndex; i1 <= maxIndex1; i1++)
				{
					bool match1 = true;
					for (int j1 = 0; j1 < valueLength1; j1++)
						if (char.ToLowerInvariant(sb[i1 + j1]) != char.ToLowerInvariant(value[j1]))
						{
							match1 = false;
							break;
						}
					if (match1)
						return i1;
				}
			}
			return -1;
		}


		/// <summary>
		/// Производит рекурсивную замену всех вхождений указанной подстроки
		/// на новую строку (включая поддержку ignoreCase).
		/// </summary>
		/// <param name="sb">Экземпляр StringBuilder.</param>
		/// <param name="oldValue">Заменяемая подстрока.</param>
		/// <param name="newValue">Новая подстрока для вставки.</param>
		/// <param name="ignoreCase">Признак игнорирования регистра символов при поиске заменяемой подстроки.</param>
		/// <returns>Текущий экземпляр StringBuilder с выполненными заменами.</returns>
		public static StringBuilder ReplaceRecursively(
			this StringBuilder sb,
			string oldValue,
			string newValue,
			bool ignoreCase)
		{
			if (string.IsNullOrEmpty(oldValue))
				return sb;
			int i1 = sb.IndexOf(oldValue, 0, ignoreCase);
			if (i1 < 0)
				return sb;
			int oldLength1 = oldValue.Length;
			int newLength1 = newValue.Length;
			while (i1 >= 0)
			{
				sb.Remove(i1, oldLength1);
				sb.Insert(i1, newValue);
				i1 = sb.IndexOf(oldValue, i1 + newLength1, ignoreCase);
			}
			return sb;
		}


		/// <summary>
		/// Удаляет все начальные и конечные пробельные символы из текущего объекта StringBuilder.
		/// </summary>
		/// <param name="sb">Модифицируемый экземпляр StringBuilder.</param>
		public static void Trim(
			this StringBuilder sb)
		{
			int length1 = sb.Length;
			if (length1 == 0)
				return;
			int start1 = 0;
			while (start1 < length1 && char.IsWhiteSpace(sb[start1]))
				start1++;
			int end1 = length1 - 1;
			while (end1 >= start1 && char.IsWhiteSpace(sb[end1]))
				end1--;
			if (end1 < start1)
				sb.Clear();
			else
			{
				if (end1 < length1 - 1)
					sb.Remove(end1 + 1, length1 - 1 - end1);
				if (start1 > 0)
					sb.Remove(0, start1);
			}
		}


		/// <summary>
		/// Удаляет все начальные и конечные вхождения указанных символов из текущего объекта StringBuilder.
		/// </summary>
		/// <param name="sb">Модифицируемый экземпляр StringBuilder.</param>
		/// <param name="chars">Набор символов, подлежащих удалению.</param>
		public static void Trim(
			this StringBuilder sb,
			ReadOnlySpan<char> chars)
		{
			int length1 = sb.Length;
			if (length1 == 0 || chars.IsEmpty)
				return;
			int start1 = 0;
			while (start1 < length1 && chars.Contains(sb[start1]))
				start1++;
			int end1 = length1 - 1;
			while (end1 >= start1 && chars.Contains(sb[end1]))
				end1--;
			if (end1 < start1)
				sb.Clear();
			else
			{
				if (end1 < length1 - 1)
					sb.Remove(end1 + 1, length1 - 1 - end1);
				if (start1 > 0)
					sb.Remove(0, start1);
			}
		}

	}

}
