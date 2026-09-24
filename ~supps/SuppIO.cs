// rev 2026-09-16

using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Перечисление поддерживаемых библиотекой кодировок текстовых файлов.
	/// </summary>
	public enum EncodingsEnum
	{
		UTF8,
		WINDOWS1251,
		KOI8R,
		CP866,
		ISO88591
	}



	/// <summary>
	/// Вспомогательный класс для работы с операциями ввода-вывода (IO).
	/// </summary>
	public static class SuppIO
	{

		//public static string GetCatalogName(
		//	int id)
		//{
		//	return $"{id:0:000/00/00}"; // string.Format("{0:000/00/00}", id);
		//}

		//public static void Register_CodePagesEncodingProvider()
		//{
		//	Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		//}


		/* functions */


		/// <summary>
		/// Возвращает системный объект кодировки <see cref="Encoding"/>
		/// на основе выбранного значения из <see cref="EncodingsEnum"/>.
		/// </summary>
		/// <param name="encoding">Вариант кодировки из перечисления.</param>
		/// <returns>Экземпляр класса <see cref="Encoding"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Encoding GetEncoding(
			EncodingsEnum encoding)
		{
			return encoding switch
			{
				EncodingsEnum.WINDOWS1251 => _Consts.ENCODING_WINDOWS1251,
				EncodingsEnum.KOI8R => _Consts.ENCODING_KOI8R,
				EncodingsEnum.CP866 => _Consts.ENCODING_CP866,
				EncodingsEnum.ISO88591 => _Consts.ENCODING_ISO88591,
				_ => _Consts.ENCODING_UTF8
			};
		}


		/// <summary>
		/// Генерирует уникальный полный путь к файлу в целевой директории. 
		/// Если файл с указанным именем уже существует, рекурсивно добавляет суффикс "_" к имени файла перед расширением.
		/// </summary>
		/// <param name="file">Информационный объект исходного файла, используемый для определения целевой директории.</param>
		/// <param name="newName">Желаемое новое имя файла с расширением.</param>
		/// <returns>Строка, содержащая уникальный полный путь к файлу.</returns>
		public static string GetNewName(
			FileInfo file,
			string newName)
		{
			var path1 = Path.Combine(file.DirectoryName ?? string.Empty, newName);
			if (!File.Exists(path1))
				return path1;
			var nameOnly1 = Path.GetFileNameWithoutExtension(path1);
			var ext1 = Path.GetExtension(path1);
			return GetNewName(file, $"{nameOnly1}_{ext1}");
		}


		/// <summary>
		/// Возвращает расширение файла из указанного пути.
		/// </summary>
		/// <param name="path">Путь к файлу или имя файла.</param>
		/// <param name="hasDot">Признак необходимости сохранения точки перед расширением (например, ".txt" вместо "txt").</param>
		/// <returns>Строка расширения в нижнем регистре. Если расширение отсутствует, возвращает пустую строку.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileExtension(
			string path,
			bool hasDot)
		{
			var s1 = Path.GetExtension(path).ToLowerInvariant();
			if (string.IsNullOrEmpty(s1))
				return string.Empty;
			return hasDot ? s1 : s1[1..];
		}


		/// <summary>
		/// Разделяет имя файла на две части: имя без расширения и само расширение (включая точку).
		/// </summary>
		/// <param name="filename">Имя файла для разделения.</param>
		/// <returns>Массив из двух элементов: [имя_без_расширения, расширение]. Если входная строка <see langword="null"/>, возвращает пустой массив.</returns>
		public static string[] GetFilenameHalfs(
			string filename)
		{
			if (filename == null)
				return [];
			int i1 = filename.LastIndexOf('.');
			return i1 == -1
				? [filename, string.Empty]
				: [filename[..i1], filename[i1..]];
		}


		/// <summary>
		/// Регистронезависимый быстрый поиск <see cref="ContentInfo"/> по расширению файла.
		/// </summary>
		/// <param name="extension">Расширение файла (с точкой или без).</param>
		/// <returns>Объект метаданных контента; если расширение неизвестно, возвращается дефолтный бинарный тип.</returns>
		public static ContentInfo GetContentInfoFromExtension(
			string extension)
		{
			if (string.IsNullOrEmpty(extension))
				return _Consts.CONTENTINFO_BIN;
			string key1 = extension[0] == '.'
				? extension
				: string.Concat(".", extension);
			return _Consts.CONTENTINFOS.TryGetValue(key1, out var info1)
				? info1
				: _Consts.CONTENTINFO_BIN;
		}


		/// <summary>
		/// Поиск <see cref="ContentInfo"/> по полному пути или имени файла.
		/// </summary>
		/// <param name="path">Путь к файлу или его имя.</param>
		/// <returns>Объект метаданных контента.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ContentInfo GetContentInfoFromPath(
			string path)
		{
			return GetContentInfoFromExtension(
				GetFileExtension(path, true));
		}


		/// <summary>
		/// Считывает весь текстовый контент из файла по указанному пути, используя выбранную кодировку.
		/// </summary>
		/// <param name="path">Путь к файлу для чтения.</param>
		/// <param name="encoding">Кодировка текста (по умолчанию UTF-8).</param>
		/// <returns>Строка, содержащая весь текст из файла.</returns>
		public static string FileRead(
			string path,
			EncodingsEnum encoding = EncodingsEnum.UTF8)
		{
			using var fs1 = new FileStream(
				path, FileMode.Open, FileAccess.Read, FileShare.Read);
			using var sr1 = new StreamReader(
				fs1, GetEncoding(encoding));
			return sr1.ReadToEnd();
		}


		/// <summary>
		/// Считывает начало файла из открытого потока до указанного размера и возвращает его в виде Base64-строки.
		/// </summary>
		/// <param name="stream">Открытый поток файла.</param>
		/// <param name="size">Максимальное количество байт для чтения (по умолчанию 255).</param>
		/// <returns>Строка в формате Base64, представляющая начало файла.</returns>
		public static string GetFileBegin(
			FileStream stream,
			int size = 255)
		{
			if (size <= 0)
				return string.Empty;
			byte[] buffer1 = new byte[size];
			int i1 = stream.Read(buffer1, 0, size);
			if (i1 <= 0)
				return string.Empty;
			return Convert.ToBase64String(buffer1, 0, i1);
		}


		/// <summary>
		/// Считывает начало файла по указанному пути до указанного размера и возвращает его в виде Base64-строки.
		/// </summary>
		/// <param name="path">Путь к файлу.</param>
		/// <param name="size">Максимальное количество байт для чтения (по умолчанию 255).</param>
		/// <returns>Строка в формате Base64, представляющая начало файла.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileBegin(
			string path,
			int size = 255)
		{
			using var stream1 = new FileStream(
				path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return GetFileBegin(stream1, size);
		}


		/// <summary>
		/// Вычисляет HMAC-SHA1 хэш для указанного потока файла с использованием байтовой соли.
		/// </summary>
		/// <param name="stream">Открытый поток файла.</param>
		/// <param name="salt">Байтовый массив соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		public static string GetFileSHA1(
			FileStream stream,
			byte[] salt)
		{
			using var alg1 = new HMACSHA1(salt);
			byte[] hash1 = alg1.ComputeHash(stream);
			return Convert.ToHexString(hash1).ToLowerInvariant();
		}


		/// <summary>
		/// Вычисляет HMAC-SHA1 хэш для указанного потока файла с использованием строковой соли (Unicode).
		/// </summary>
		/// <param name="stream">Открытый поток файла.</param>
		/// <param name="salt">Строковое значение соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileSHA1(
			FileStream stream,
			string salt)
		{
			return GetFileSHA1(stream, Encoding.Unicode.GetBytes(salt));
		}


		/// <summary>
		/// Вычисляет HMAC-SHA1 хэш для файла по указанному пути с использованием байтовой соли.
		/// </summary>
		/// <param name="path">Путь к хэшируемому файлу.</param>
		/// <param name="salt">Байтовый массив соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileSHA1(
			string path,
			byte[] salt)
		{
			using var stream1 = new FileStream(
				path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return GetFileSHA1(stream1, salt);
		}


		/// <summary>
		/// Вычисляет HMAC-SHA1 хэш для файла по указанному пути с использованием строковой соли.
		/// </summary>
		/// <param name="path">Путь к хэшируемому файлу.</param>
		/// <param name="salt">Строковое значение соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileSHA1(
			string path,
			string salt)
		{
			return GetFileSHA1(path, Encoding.Unicode.GetBytes(salt));
		}


		/// <summary>
		/// Вычисляет HMAC-SHA256 хэш для указанного потока файла с использованием байтовой соли.
		/// </summary>
		/// <param name="stream">Открытый поток файла.</param>
		/// <param name="salt">Байтовый массив соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		public static string GetFileSHA256(
			FileStream stream,
			byte[] salt)
		{
			using var alg1 = new HMACSHA256(salt);
			byte[] hash1 = alg1.ComputeHash(stream);
			return Convert.ToHexString(hash1).ToLowerInvariant();
		}


		/// <summary>
		/// Вычисляет HMAC-SHA256 хэш для указанного потока файла с использованием строковой соли (Unicode).
		/// </summary>
		/// <param name="stream">Открытый поток файла.</param>
		/// <param name="salt">Строковое значение соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileSHA256(
			FileStream stream,
			string salt)
		{
			return GetFileSHA256(stream, Encoding.Unicode.GetBytes(salt));
		}


		/// <summary>
		/// Вычисляет HMAC-SHA256 хэш для файла по указанному пути с использованием байтовой соли.
		/// </summary>
		/// <param name="path">Путь к хэшируемому файлу.</param>
		/// <param name="salt">Байтовый массив соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileSHA256(
			string path,
			byte[] salt)
		{
			using var stream1 = new FileStream(
				path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return GetFileSHA256(stream1, salt);
		}


		/// <summary>
		/// Вычисляет HMAC-SHA256 хэш для файла по указанному пути с использованием строковой соли.
		/// </summary>
		/// <param name="path">Путь к хэшируемому файлу.</param>
		/// <param name="salt">Строковое значение соли.</param>
		/// <returns>Шестнадцатеричная строка хэша в нижнем регистре.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetFileSHA256(
			string path,
			string salt)
		{
			return GetFileSHA256(path, Encoding.Unicode.GetBytes(salt));
		}









		/// <summary>
		/// Возвращает время последнего изменения файла с указанием смещения (DateTimeOffset).
		/// </summary>
		/// <param name="filename">Путь к файлу.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static DateTimeOffset GetFileLastModified(
			string filename)
		{
			var time1 = File.GetLastWriteTimeUtc(filename);
			return new DateTimeOffset(time1);
		}


		/// <summary>
		/// Возвращает самое последнее (максимальное) время изменения файлов в указанном каталоге.
		/// </summary>
		/// <param name="directory">Информационный объект каталога.</param>
		public static DateTime GetLastWriteTimeFiles(
			DirectoryInfo directory)
		{
			var date1 = directory.LastWriteTime;
			foreach (var item1 in directory.GetFiles())
				if (item1.LastWriteTime > date1)
					date1 = item1.LastWriteTime;
			return date1;
		}


		/// <summary>
		/// Возвращает строковое представление размера данных в КБ, округленное в большую сторону по маске ресурса.
		/// </summary>
		/// <param name="length">Размер в байтах.</param>
		public static string GetLengthOfKB(
			long length)
		{
			long l1 = 1;
			if (length >= 1024)
				l1 = (length + 1023) / 1024;
			return l1.ToString(Resources.Common.Format_LengthKB).TrimStart();
		}


		/// <summary>
		/// Возвращает строковое представление размера данных в КБ для 32-битного целого числа.
		/// </summary>
		/// <param name="length">Размер в байтах.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetLengthOfKB(
			int length)
		{
			return GetLengthOfKB((long)length);
		}


		/// <summary>
		/// Экранирует зарезервированные операционной системой имена файлов (например, CON, PRN),
		/// оборачивая их в символы подчеркивания.
		/// </summary>
		/// <param name="name">Проверяемое имя файла или расширение.</param>
		public static string FixForbiddenFileName(
			string name)
		{
			return (name.Length < 5 && _Consts.FORBIDDEN_FILE_NAMES.Contains(name))
				? $"_{name}_" : name;
		}


		/// <summary>
		/// Возвращает нормализованное безопасное расширение файла в нижнем регистре без точки,
		/// приводя редкие форматы к стандартным.
		/// </summary>
		/// <param name="filename">Имя файла или путь.</param>
		public static string GetSafeFileExtension(
			string filename)
		{
			var s1 = GetFileExtension(filename, false);
			var s2 = FixForbiddenFileName(s1);
			var s3 = SuppString.GetSafeFsString(s2);
			return s3 switch
			{
				"jpeg" => "jpg",
				"jpe" => "jpg",
				"mpeg" => "mpg",
				"tiff" => "tif",
				_ => s3
			};
		}


		/// <summary>
		/// Возвращает безопасное имя файла без расширения. Если имя превышает 80 символов,
		/// сокращает его и добавляет уникальный хэш.
		/// </summary>
		/// <param name="filename">Исходное имя файла.</param>
		public static string GetSafeFileNameWithoutExtension(
			string filename)
		{
			var s1 = Path.GetFileNameWithoutExtension(filename);
			var s2 = FixForbiddenFileName(s1);
			var s3 = SuppString.GetSafeFsString(s2);
			if (s3.Length > 80)
				s3 = $"{s3[..38]}{SuppCrypto.ComputeSha256(s1)}";
			return s3;
		}


		/// <summary>
		/// Формирует полностью безопасное имя файла, очищенное от запрещенных символов и системных ограничений.
		/// </summary>
		/// <param name="filename">Исходное имя файла.</param>
		public static string GetSafeFilename(
			string filename)
		{
			if (string.IsNullOrEmpty(filename))
				return string.Empty;
			var ext1 = GetSafeFileExtension(filename);
			var name1 = GetSafeFileNameWithoutExtension(filename);
			return $"{name1}.{ext1}";
		}


		/// <summary>
		/// Проверяет, содержит ли указанный путь запрещенные символы файловой системы.
		/// </summary>
		/// <param name="path">Проверяемый путь.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasInvalidPathChars(
			string path)
		{
			return path.IndexOfAny(Path.GetInvalidPathChars()) >= 0;
		}


		/// <summary>
		/// Проверяет, содержит ли указанное имя файла запрещенные символы.
		/// </summary>
		/// <param name="filename">Проверяемое имя файла.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasInvalidFileNameChars(
			string filename)
		{
			return filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
		}


		/// <summary>
		/// Выполняет полное побайтовое сравнение содержимого двух файлов. Имена и пути файлов могут отличаться.
		/// </summary>
		/// <param name="file1">Первый сравниваемый файл.</param>
		/// <param name="file2">Второй сравниваемый файл.</param>
		/// <returns><see langword="true"/>, если содержимое файлов абсолютно идентично; иначе — <see langword="false"/>.</returns>
		public static bool IsFilesEqualFull(
			FileInfo file1,
			FileInfo file2)
		{
			if (file1.Length != file2.Length)
				return false;
			if (string.Equals(file1.FullName, file2.FullName, StringComparison.OrdinalIgnoreCase))
				return true;
			using var stream1 = file1.OpenRead();
			using var stream2 = file2.OpenRead();
			const int _BUF_SIZE = 4096;
			byte[] buffer1 = new byte[_BUF_SIZE];
			byte[] buffer2 = new byte[_BUF_SIZE];
			while (true)
			{
				int count1 = stream1.Read(buffer1, 0, _BUF_SIZE);
				int count2 = stream2.Read(buffer2, 0, _BUF_SIZE);
				if (count1 != count2)
					return false;
				if (count1 == 0)
					break;
				if (!buffer1.AsSpan(0, count1).SequenceEqual(buffer2.AsSpan(0, count2)))
					return false;
			}
			return true;
		}


		/// <summary>
		/// Выполняет быстрое «ленивое» сравнение двух файлов по размеру и времени изменения. 
		/// Если метаданные не помогли определить равенство, выполняет побайтовое сравнение содержимого.
		/// </summary>
		/// <param name="file1">Первый сравниваемый файл.</param>
		/// <param name="file2">Второй сравниваемый файл.</param>
		/// <returns><see langword="true"/>, если файлы идентичны; иначе — <see langword="false"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsFilesEqualLazy(
			FileInfo file1,
			FileInfo file2)
		{
			if (file1.Length == file2.Length
				&& file1.LastWriteTimeUtc == file2.LastWriteTimeUtc)
				return true;
			return IsFilesEqualFull(file1, file2);
		}


		/* methods */


		/// <summary>
		/// Создает директорию по указанному пути, если она еще не существует.
		/// </summary>
		/// <param name="path">Путь к создаваемой директории.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CreateDirectoryIfNotExists(
			string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}


		/// <summary>
		/// Удаляет директорию и все ее содержимое по указанному пути, если она существует.
		/// </summary>
		/// <param name="path">Путь к удаляемой директории.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DeleteDirectoryIfExists(
			string path)
		{
			if (Directory.Exists(path))
				Directory.Delete(path, true);
		}


		/// <summary>
		/// Удаляет файл по указанному пути, если он существует.
		/// </summary>
		/// <param name="path">Путь к удаляемому файлу.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DeleteFileIfExists(
			string path)
		{
			if (File.Exists(path))
				File.Delete(path);
		}


		/// <summary>
		/// Переименовывает файл, безопасно перемещая его в новое имя. 
		/// Если целевое имя уже занято, автоматически подбирает уникальное имя с помощью <see cref="GetNewName"/>.
		/// </summary>
		/// <param name="file">Переименовываемый файл.</param>
		/// <param name="newName">Новое имя файла.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Rename(
			FileInfo file,
			string newName)
		{
			var uniquePath = GetNewName(file, newName);
			file.MoveTo(uniquePath);
		}


		/// <summary>
		/// Записывает строковый контент в файл по указанному пути,
		/// используя выбранную кодировку и режим открытия файла.
		/// </summary>
		/// <param name="path">Путь к файлу для записи.</param>
		/// <param name="content">Строковое содержимое, которое необходимо записать.</param>
		/// <param name="encoding">Кодировка текста (по умолчанию UTF-8).</param>
		/// <param name="mode">Режим открытия или создания файла (по умолчанию перезапись/создание).</param>
		public static void FileWrite(
			string path,
			string content,
			EncodingsEnum encoding = EncodingsEnum.UTF8,
			FileMode mode = FileMode.Create)
		{
			using var fs1 = new FileStream(path, mode);
			using var sw1 = new StreamWriter(fs1, GetEncoding(encoding));
			sw1.Write(content);
		}


		/// <summary>
		/// Записывает массив байт в файл по указанному пути, используя выбранный режим открытия файла.
		/// </summary>
		/// <param name="path">Путь к файлу для записи.</param>
		/// <param name="content">Массив байт, который необходимо записать.</param>
		/// <param name="mode">Режим открытия или создания файла (по умолчанию перезапись/создание).</param>
		public static void FileWrite(
			string path,
			byte[] content,
			FileMode mode = FileMode.Create)
		{
			using var fs1 = new FileStream(path, mode);
			fs1.Write(content);
		}

	}

}
