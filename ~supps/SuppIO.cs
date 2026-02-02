using System.Security.Cryptography;
using System.Text;

namespace Ans.Net10.Common
{

	public enum EncodingsEnum
	{
		UTF8,
		WINDOWS1251,
		KOI8R,
		CP866,
		ISO88591,
	}



	public static class SuppIO
	{

		//public static string GetCatalogName(
		//	int id)
		//{
		//	return $"{id:0:000/00/00}"; // string.Format("{0:000/00/00}", id);
		//}


		/* methods */


		public static void Register_CodePagesEncodingProvider()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}


		/// <summary>
		/// Создает директорию, если ее не существует
		/// </summary>
		public static void CreateDirectoryIfNotExists(
			string path)
		{
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
		}


		/// <summary>
		/// Удаляет директорию и все ее содержимое, если она существует
		/// </summary>
		public static void DeleteDirectoryIfExists(
			string path)
		{
			if (Directory.Exists(path))
				Directory.Delete(path, true);
		}


		/// <summary>
		/// Удаляет файл, если он существует
		/// </summary>
		public static void DeleteFileIfExists(
			string path)
		{
			if (File.Exists(path))
				File.Delete(path);
		}


		public static string GetNewName(
			FileInfo file,
			string newName)
		{
			var path1 = $"{file.Directory}/{newName}";
			if (!File.Exists(path1))
				return path1;
			var name1 = Path.GetFileName(path1);
			var ext1 = Path.GetExtension(path1);
			return GetNewName(file, $"{name1}_.{ext1}");
		}


		public static void Rename(
			FileInfo file,
			string newName)
		{
			var s1 = GetNewName(file, newName);
			file.MoveTo(s1);
		}


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


		public static void FileWrite(
			string path,
			byte[] content,
			FileMode mode)
		{
			using var fs1 = new FileStream(path, mode);
			fs1.Write(content, 0, content.Length);
		}


		/* functions */


		public static ContentInfo GetContentInfoFromExtention(
			string extention)
		{
			return _Consts.CONTENTINFOS.FirstOrDefault(
				x => x.Extention.Equals(extention, StringComparison.InvariantCultureIgnoreCase))
					?? _Consts.CONTENTINFO_BIN;
		}


		public static ContentInfo GetContentInfoFromPath(
			string path)
		{
			return GetContentInfoFromExtention(
				GetFileExtension(path, true));
		}


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
		/// Возвращает относительное время последнего изменения файла
		/// </summary>
		public static DateTimeOffset GetFileLastModified(
			string filename)
		{
			var time1 = File.GetLastWriteTimeUtc(filename);
			return new DateTimeOffset(time1);
		}


		/// <summary>
		/// Возвращает самое последнее время изменения файлов в каталоге
		/// </summary>
		public static DateTime GetLastWriteTimeFiles(
			DirectoryInfo directory)
		{
			var date1 = directory.LastWriteTime;
			foreach (var item1 in directory.GetFiles())
				if (item1.LastWriteTime < date1)
					date1 = item1.LastWriteTime;
			return date1;
		}


		/// <summary>
		/// Возвращает расширение файла из path
		/// </summary>
		public static string GetFileExtension(
			string path,
			bool hasDot)
		{
			string s1 = Path.GetExtension(path).ToLower();
			return (hasDot)
				? s1 : (!string.IsNullOrEmpty(s1))
					? s1[1..] : null;
		}


		/// <summary>
		/// Возвращает части имени файла разделенные '.'
		/// </summary>
		public static string[] GetFilenameHalfs(
			string filename)
		{
			if (filename == null)
				return null;
			int i1 = filename.LastIndexOf('.');
			return (i1 == -1)
				? [filename, string.Empty]
				: [filename[..i1], filename[(i1 + 1)..]];
		}


		public static string GetFileBegin(
			FileStream stream,
			int size = 255)
		{
			byte[] a1 = new byte[size];
			_ = stream.Read(a1, 0, size);
			return Convert.ToBase64String(a1);
		}


		public static string GetFileBegin(
			string path,
			int size = 255)
		{
			using var stream1 = new FileStream(
				path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return GetFileBegin(stream1, size);
		}


		public static string GetFileSHA1(
			FileStream stream,
			byte[] salt)
		{
			using var alg1 = new HMACSHA1(salt);
			try
			{
				byte[] a1 = alg1.ComputeHash(stream);
				return string.Join(
					"", a1.Select(x => x.ToString("x2"))); // Convert.ToBase64String(result);
			}
			finally { alg1.Clear(); }
		}


		public static string GetFileSHA1(
			FileStream stream,
			string salt)
		{
			return GetFileSHA1(stream, Encoding.Unicode.GetBytes(salt));
		}


		public static string GetFileSHA1(
			FileStream stream)
		{
			byte[] a1 = [];
			return GetFileSHA1(stream, a1);
		}


		public static string GetFileSHA1(
			string path,
			byte[] salt)
		{
			using var stream1 = new FileStream(
				path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return GetFileSHA1(stream1, salt);
		}


		public static string GetFileSHA1(
			string path,
			string salt)
		{
			return GetFileSHA1(path, Encoding.Unicode.GetBytes(salt));
		}


		public static string GetLengthOfKB(
			long length)
		{
			long l1 = 1;
			if (length >= 1024)
				l1 = (long)Math.Ceiling((decimal)length / 1024);
			return l1.ToString(Resources.Common.Format_LengthKB).TrimStart();
		}


		public static string GetLengthOfKB(
			int length)
		{
			return GetLengthOfKB((long)length);
		}


		public static string FixForbiddenFileName(
			string name)
		{
			return (name.Length < 5 && _Consts.FORBIDDEN_FILE_NAMES.Contains(name))
				? $"_{name}_" : name;
		}


		public static string GetSafeFimeExtension(
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


		public static string GetSafeFileNameWithoutExtension(
			string filename)
		{
			var s1 = Path.GetFileNameWithoutExtension(filename);
			var s2 = FixForbiddenFileName(s1);
			var s3 = SuppString.GetSafeFsString(s2);
			if (s3.Length > 80)
				s3 = $"{s3[..38]}{SuppCrypto.GetHashString(s1, "safe")}";
			return s3;
		}


		public static string GetSafeFilename(
			string filename)
		{
			if (filename == null)
				return null;
			var ext1 = GetSafeFimeExtension(filename);
			var name1 = GetSafeFileNameWithoutExtension(filename);
			return $"{name1}.{ext1}";
		}


		public static bool HasInvalidPathChars(
			string path)
		{
			return path.IndexOfAny(Path.GetInvalidPathChars()) >= 0;
		}


		public static bool HasInvalidFileNameChars(
			string filename)
		{
			return filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;
		}


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
		/// Сравнение файлов
		/// </summary>
		public static bool IsFilesEqual(
			FileInfo file1,
			FileInfo file2)
		{
			if (file1.Length != file2.Length)
				return false;
			using var stream1 = file1.OpenRead();
			using var stream2 = file2.OpenRead();
			if (!GetFileBegin(stream1).Equals(GetFileBegin(stream2)))
				return false;
			if (!GetFileSHA1(stream1).Equals(GetFileSHA1(stream2)))
				return false;
			return true;
		}

	}

}
