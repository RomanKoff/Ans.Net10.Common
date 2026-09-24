// rev 2026-09-15

using System.Runtime.CompilerServices;

namespace Ans.Net10.Common
{

	public static partial class Exts__collections
	{

		/// <summary>
		/// Возвращает новый массив, добавляя указанный элемент в конец исходного массива.
		/// </summary>
		/// <typeparam name="T">Тип элементов массива.</typeparam>
		/// <param name="source">Исходный массив.</param>
		/// <param name="item">Элемент для добавления.</param>
		/// <returns>Новый массив, содержащий все исходные элементы и добавленный элемент.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] GetArrayAdd<T>(
			this T[]? source,
			T item)
		{
			if (source == null || source.Length == 0)
				return [item];
			return [.. source, item];
		}


		/// <summary>
		/// Возвращает новый массив, вставляя указанный элемент по заданному индексу.
		/// </summary>
		/// <typeparam name="T">Тип элементов массива.</typeparam>
		/// <param name="source">Исходный массив.</param>
		/// <param name="index">Индекс, по которому необходимо вставить элемент.</param>
		/// <param name="item">Элемент для вставки.</param>
		/// <returns>Новый массив с вставленным элементом.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Вызывается, если индекс находится вне диапазонов массива.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] GetArrayInsert<T>(
			this T[]? source,
			int index,
			T item)
		{
			int len1 = source?.Length ?? 0;
			if (index < 0 || index > len1)
				throw new ArgumentOutOfRangeException(
					nameof(index), "The index is outside the boundaries of the array.");
			if (source == null || len1 == 0)
				return [item];
			return [.. source[..index], item, .. source[index..]];
		}


		/// <summary>
		/// Возвращает новый массив, удаляя элемент по указанному индексу.
		/// </summary>
		/// <typeparam name="T">Тип элементов массива.</typeparam>
		/// <param name="source">Исходный массив.</param>
		/// <param name="index">Индекс элемента для удаления.</param>
		/// <returns>Новый массив без удаленного элемента.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Вызывается, если индекс находится вне границ массива.</exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] GetArrayRemoveAt<T>(
			this T[]? source,
			int index)
		{
			if (source == null
				|| source.Length == 0
				|| index < 0
				|| index >= source.Length)
				throw new ArgumentOutOfRangeException(
					nameof(index), "The index is outside the boundaries of the array.");
			if (source.Length == 1)
				return [];
			return [.. source[..index], .. source[(index + 1)..]];
		}


		/// <summary>
		/// Возвращает новый массив, удаляя первое вхождение указанного элемента. 
		/// Если элемент не найден, возвращает исходный массив.
		/// </summary>
		/// <typeparam name="T">Тип элементов массива.</typeparam>
		/// <param name="source">Исходный массив.</param>
		/// <param name="item">Элемент, который необходимо удалить.</param>
		/// <returns>Новый массив без удаленного элемента или исходный массив.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] GetArrayRemove<T>(
			this T[]? source,
			T item)
		{
			if (source == null || source.Length == 0)
				return [];
			int index1 = Array.IndexOf(source, item);
			if (index1 == -1)
				return source;
			return source.GetArrayRemoveAt(index1);
		}


		/// <summary>
		/// Синхронизирует коллекции и возвращает изменения в виде именованного кортежа с ленивыми потоками.
		/// </summary>
		/// <typeparam name="T">Тип элементов коллекций.</typeparam>
		/// <typeparam name="TKey">Тип ключа элементов.</typeparam>
		/// <param name="current">Текущая коллекция элементов.</param>
		/// <param name="newest">Новая коллекция элементов.</param>
		/// <param name="keySelector">Функция извлечения ключа из элемента.</param>
		/// <param name="comparer">Компаратор для сравнения ключей. Если не задан, используется компаратор по умолчанию.</param>
		/// <returns>Именованный кортеж, содержащий ленивые потоки добавленных, удаленных и обновленных элементов.</returns>
		public static
			(IEnumerable<T> Added, IEnumerable<T> Removed, IEnumerable<T> Updated)
			GetDiffLazy<T, TKey>(
				this IEnumerable<T> current,
				IEnumerable<T> newest,
				Func<T, TKey> keySelector,
				IEqualityComparer<TKey>? comparer = null)
			where TKey : notnull
		{
			comparer ??= EqualityComparer<TKey>.Default;
			var currentKeys1 = current.Select(keySelector).ToHashSet(comparer);
			var newestKeys1 = newest.Select(keySelector).ToHashSet(comparer);
			var added1 = newest.Where(x => !currentKeys1.Contains(keySelector(x)));
			var removed1 = current.Where(x => !newestKeys1.Contains(keySelector(x)));
			var updated1 = newest.Where(x => currentKeys1.Contains(keySelector(x)));
			return (added1, removed1, updated1);
		}


		/// <summary>
		/// Синхронизирует коллекции и возвращает изменения в виде именованного кортежа с материализованными списками.
		/// </summary>
		/// <typeparam name="T">Тип элементов коллекций.</typeparam>
		/// <typeparam name="TKey">Тип ключа элементов.</typeparam>
		/// <param name="current">Текущая коллекция элементов.</param>
		/// <param name="newest">Новая коллекция элементов.</param>
		/// <param name="keySelector">Функция извлечения ключа из элемента.</param>
		/// <param name="comparer">Компаратор для сравнения ключей. Если не задан, используется компаратор по умолчанию.</param>
		/// <returns>Именованный кортеж, содержащий списки добавленных, удаленных и обновленных элементов.</returns>
		public static
			(List<T> Added, List<T> Removed, List<T> Updated)
			GetDiffList<T, TKey>(
				this IEnumerable<T> current,
				IEnumerable<T> newest,
				Func<T, TKey> keySelector,
				IEqualityComparer<TKey>? comparer = null)
			where TKey : notnull
		{
			comparer ??= EqualityComparer<TKey>.Default;
			var newestMap1 = new Dictionary<TKey, T>(comparer);
			foreach (var item1 in newest)
				newestMap1[keySelector(item1)] = item1;
			var added1 = new List<T>();
			var updated1 = new List<T>();
			var removed1 = new List<T>();
			var currentKeys1 = new HashSet<TKey>(comparer);
			foreach (var item1 in current)
			{
				var key1 = keySelector(item1);
				currentKeys1.Add(key1);
				if (newestMap1.TryGetValue(key1, out var newItem1))
					updated1.Add(newItem1);
				else
					removed1.Add(item1);
			}
			foreach (var kvp1 in newestMap1)
				if (!currentKeys1.Contains(kvp1.Key))
					added1.Add(kvp1.Value);
			return (added1, removed1, updated1);
		}


		/// <summary>
		/// Возвращает текущие версии элементов, которые все еще присутствуют в новой коллекции.
		/// </summary>
		/// <typeparam name="T">Тип элементов коллекций.</typeparam>
		/// <typeparam name="TKey">Тип ключа элементов.</typeparam>
		/// <param name="current">Текущая коллекция элементов.</param>
		/// <param name="newest">Новая коллекция элементов.</param>
		/// <param name="keySelector">Функция извлечения ключа из элемента.</param>
		/// <returns>Ленивый поток актуальных текущих элементов.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<T> GetActual<T, TKey>(
			this IEnumerable<T> current,
			IEnumerable<T> newest,
			Func<T, TKey> keySelector)
			where TKey : notnull
		{
			return current.IntersectBy(
				newest.Select(keySelector), keySelector);
		}


		/// <summary>
		/// Возвращает обновленные версии элементов из новой коллекции, которые уже существовали в текущей коллекции.
		/// </summary>
		/// <typeparam name="T">Тип элементов коллекций.</typeparam>
		/// <typeparam name="TKey">Тип ключа элементов.</typeparam>
		/// <param name="current">Текущая коллекция элементов.</param>
		/// <param name="newest">Новая коллекция элементов.</param>
		/// <param name="keySelector">Функция извлечения ключа из элемента.</param>
		/// <returns>Ленивый поток обновленных версий элементов.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<T> GetUpdated<T, TKey>(
			this IEnumerable<T> current,
			IEnumerable<T> newest,
			Func<T, TKey> keySelector)
			where TKey : notnull
		{
			return newest.IntersectBy(
				current.Select(keySelector), keySelector);
		}


		/// <summary>
		/// Возвращает элементы, которые появились в новой коллекции, но отсутствовали в текущей.
		/// </summary>
		/// <typeparam name="T">Тип элементов коллекций.</typeparam>
		/// <typeparam name="TKey">Тип ключа элементов.</typeparam>
		/// <param name="current">Текущая коллекция элементов.</param>
		/// <param name="newest">Новая коллекция элементов.</param>
		/// <param name="keySelector">Функция извлечения ключа из элемента.</param>
		/// <returns>Ленивый поток добавленных элементов.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<T> GetAdded<T, TKey>(
			this IEnumerable<T> current,
			IEnumerable<T> newest,
			Func<T, TKey> keySelector)
			where TKey : notnull
		{
			return newest.ExceptBy(
				current.Select(keySelector), keySelector);
		}


		/// <summary>
		/// Возвращает элементы, которые присутствовали в текущей коллекции, но отсутствуют в новой.
		/// </summary>
		/// <typeparam name="T">Тип элементов коллекций.</typeparam>
		/// <typeparam name="TKey">Тип ключа элементов.</typeparam>
		/// <param name="current">Текущая коллекция элементов.</param>
		/// <param name="newest">Новая коллекция элементов.</param>
		/// <param name="keySelector">Функция извлечения ключа из элемента.</param>
		/// <returns>Ленивый поток удаленных элементов.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<T> GetRemoved<T, TKey>(
			this IEnumerable<T> current,
			IEnumerable<T> newest,
			Func<T, TKey> keySelector)
			where TKey : notnull
		{
			return current.ExceptBy(
				newest.Select(keySelector), keySelector);
		}


		/// <summary>
		/// Очищает элементы последовательности строк от начальных и конечных пробелов, удаляя пустые элементы и null.
		/// </summary>
		/// <param name="source">Исходная последовательность строк, допускающих значение null.</param>
		/// <returns>Ленивый поток очищенных непустых строк.</returns>
		public static IEnumerable<string> GetItemsClean(
			this IEnumerable<string?> source)
		{
			foreach (var item1 in source)
			{
				if (item1 is null)
					continue;
				var s1 = item1.Trim();
				if (s1.Length > 0)
					yield return s1;
			}
		}


		/// <summary>
		/// Обрезает начальные и конечные пробелы у всех элементов последовательности строк.
		/// </summary>
		/// <param name="source">Исходная последовательность строк, допускающих значение null.</param>
		/// <returns>Последовательность строк с обрезанными пробелами, где null-элементы сохраняются.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<string?> GetItemsTrim(
			this IEnumerable<string?> source)
		{
			return source.Select(
				x => x?.Trim());
		}


		/// <summary>
		/// Исключает из последовательности строк все пустые элементы и элементы со значением null.
		/// </summary>
		/// <param name="source">Исходная последовательность строк, допускающих значение null.</param>
		/// <returns>Последовательность строк, не содержащая null и пустые значения.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<string> GetItemsWithoutEmpty(
			this IEnumerable<string?> source)
		{
			return source.Where(
				x => !string.IsNullOrEmpty(x))!;
		}


		/// <summary>
		/// Возвращает уникальные элементы последовательности строк с возможностью настройки сравнения.
		/// </summary>
		/// <param name="source">Исходная последовательность строк, допускающих значение null.</param>
		/// <param name="comparer">Компаратор для определения уникальности строк. Если не задан, используется Ordinal.</param>
		/// <returns>Последовательность уникальных строк.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IEnumerable<string?> GetItemsUnique(
			this IEnumerable<string?> source,
			StringComparer? comparer = null)
		{
			return source.Distinct(
				comparer ?? StringComparer.Ordinal);
		}


		/// <summary>
		/// Очищает элементы от пробелов, удаляет пустые/null значения и возвращает только уникальные строки.
		/// </summary>
		/// <param name="source">Исходная последовательность строк, допускающих значение null.</param>
		/// <param name="comparer">Компаратор для определения уникальности строк. Если не задан, используется Ordinal.</param>
		/// <returns>Ленивый поток очищенных уникальных непустых строк.</returns>
		public static IEnumerable<string> GetItemsCleanUnique(
			this IEnumerable<string?> source,
			StringComparer? comparer = null)
		{
			comparer ??= StringComparer.Ordinal;
			var seen1 = new HashSet<string>(comparer);
			foreach (var item1 in source)
			{
				if (item1 is null)
					continue;
				var s1 = item1.Trim();
				if (s1.Length > 0 && seen1.Add(s1))
					yield return s1;
			}
		}

	}

}
