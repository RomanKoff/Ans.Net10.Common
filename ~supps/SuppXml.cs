// rev 2026-09-22

using System.Collections.Concurrent;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для высокопроизводительной сериализации и десериализации XML.
	/// Поддерживает кэширование сериализаторов и оптимизированную работу с большими файлами.
	/// </summary>
	public static class SuppXml
	{

		/* functions */


		/// <summary>
		/// Сериализует объект в XML-строку.
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для сериализации.</param>
		/// <param name="namespaces">
		/// Пользовательские пространства имен XML. Если не заданы, пространства имен опускаются.
		/// </param>
		/// <param name="useFormatted">
		/// Признак форматирования результирующего XML-текста (отступы и переносы).
		/// </param>
		/// <returns>
		/// Строка, содержащая XML-представление объекта, или <see langword="null"/>,
		/// если объект равен <see langword="null"/>.
		/// </returns>
		public static string? GetXmlStringFromObject<T>(
			T? obj,
			XmlSerializerNamespaces? namespaces = null,
			bool useFormatted = false)
		{
			if (obj is null)
				return null;
			var serializer1 = _getCachedSerializer(typeof(T));
			var sb1 = new StringBuilder();
			var settings1 = _getWriterSettings(useFormatted, omitXmlDeclaration: true);
			using var writer1 = XmlWriter.Create(sb1, settings1);
			serializer1.Serialize(
				writer1, obj, namespaces ?? _emptyNamespaces);
			return sb1.ToString();
		}


		/// <summary>
		/// Сериализует объект в структуру <see cref="XDocument"/>.
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для сериализации.</param>
		/// <param name="namespaces">
		/// Пользовательские пространства имен XML. Если не заданы, пространства имен опускаются.
		/// </param>
		/// <returns>
		/// Объект <see cref="XDocument"/> или <see langword="null"/>,
		/// если исходный объект равен <see langword="null"/>.
		/// </returns>
		public static XDocument? GetXDocumentFromObject<T>(
			T? obj,
			XmlSerializerNamespaces? namespaces = null)
		{
			if (obj is null)
				return null;
			var doc1 = new XDocument();
			using var writer1 = doc1.CreateWriter();
			_getCachedSerializer(typeof(T)).Serialize(
				writer1, obj, namespaces ?? _emptyNamespaces);
			return doc1;
		}


		/// <summary>
		/// Десериализует объект из XML-строки.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="source">Исходная XML-строка.</param>
		/// <param name="defaultNamespace">Пространство имен XML по умолчанию.</param>
		/// <returns>
		/// Десериализованный объект типа <typeparamref name="T"/> или значение
		/// по умолчанию, если строка пуста.
		/// </returns>
		public static T? GetObjectFromXmlString<T>(
			string source,
			string? defaultNamespace = null)
		{
			if (string.IsNullOrWhiteSpace(source))
				return default;
			var serializer1 = _getCachedSerializer(typeof(T), defaultNamespace);
			using var reader1 = new StringReader(source);
			return (T?)serializer1.Deserialize(reader1);
		}


		/// <summary>
		/// Десериализует объект из структуры <see cref="XDocument"/>.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="source">Исходный объект <see cref="XDocument"/>.</param>
		/// <param name="defaultNamespace">Пространство имен XML по умолчанию.</param>
		/// <returns>
		/// Десериализованный объект типа <typeparamref name="T"/> или значение по умолчанию,
		/// если документ равен <see langword="null"/>.
		/// </returns>
		public static T? GetObjectFromXDocument<T>(
			XDocument source,
			string? defaultNamespace = null)
		{
			if (source is null)
				return default;
			using var reader1 = source.CreateReader();
			return (T?)_getCachedSerializer(typeof(T), defaultNamespace)
				.Deserialize(reader1);
		}


		/// <summary>
		/// Десериализует объект из входящего потока данных <see cref="Stream"/>.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="stream">Входящий поток, содержащий XML-данные.</param>
		/// <param name="defaultNamespace">Пространство имен XML по умолчанию.</param>
		/// <returns>
		/// Десериализованный объект типа <typeparamref name="T"/> или значение по умолчанию,
		/// если поток равен <see langword="null"/>.
		/// </returns>
		public static T? GetObjectFromXmlStream<T>(
			Stream stream,
			string? defaultNamespace = null)
		{
			if (stream is null)
				return default;
			using var reader1 = XmlReader.Create(stream);
			return (T?)_getCachedSerializer(typeof(T), defaultNamespace)
				.Deserialize(reader1);
		}


		/// <summary>
		/// (async) Асинхронно десериализует XML-файл небольшого или среднего размера в объект.
		/// Перед обработкой файл целиком копируется в оперативную память для минимизации удержания дескриптора файла.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="filename">Полный путь к XML-файлу на диске.</param>
		/// <param name="encoding">Кодировка текстового файла. Если не задана, определяется автоматически.</param>
		/// <param name="defaultNamespace">Пространство имен XML по умолчанию.</param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		/// <exception cref="FileNotFoundException">Вызывается, если файл отсутствует на диске.</exception>
		public static async Task<T?> GetSoftObjectFromXmlFileAsync<T>(
			string filename,
			Encoding? encoding = null,
			string? defaultNamespace = null)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(filename);
			if (!File.Exists(filename))
				throw new FileNotFoundException("XML file not found.", filename);
			using var fs1 = new FileStream(
				filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
			using var stream1 = new MemoryStream();
			await fs1.CopyToAsync(stream1);
			stream1.Position = 0;
			var settings1 = _getReaderSettings();
			var serializer1 = _getCachedSerializer(typeof(T), defaultNamespace);
			if (encoding is not null)
			{
				using var reader1 = new StreamReader(stream1, encoding);
				using var xml1 = XmlReader.Create(reader1, settings1);
				return (T?)serializer1.Deserialize(xml1);
			}
			else
			{
				using var xml1 = XmlReader.Create(stream1, settings1);
				return (T?)serializer1.Deserialize(xml1);
			}
		}


		/// <summary>
		/// (async) Асинхронно десериализует большой XML-файл в объект, используя потоковую обработку
		/// в выделенном фоновом потоке. Рекомендуется для файлов крупного размера (более 10–20 МБ)
		/// во избежание фрагментации оперативной памяти.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="filename">Полный путь к XML-файлу на диске.</param>
		/// <param name="encoding">Кодировка текстового файла. Если не задана, определяется автоматически.</param>
		/// <param name="defaultNamespace">Пространство имен XML по умолчанию.</param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		/// <exception cref="FileNotFoundException">Вызывается, если файл отсутствует на диске.</exception>
		public static async Task<T?> GetHardObjectFromXmlFileAsync<T>(
			string filename,
			Encoding? encoding = null,
			string? defaultNamespace = null)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(filename);
			if (!File.Exists(filename))
				throw new FileNotFoundException("XML file not found.", filename);
			return await Task.Run(() =>
			{
				using var stream1 = new FileStream(
					filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: false);
				var settings1 = _getReaderSettings();
				var serializer1 = _getCachedSerializer(typeof(T), defaultNamespace);
				if (encoding is not null)
				{
					using var reader1 = new StreamReader(stream1, encoding);
					using var xml1 = XmlReader.Create(reader1, settings1);
					return (T?)serializer1.Deserialize(xml1);
				}
				else
				{
					using var xml1 = XmlReader.Create(stream1, settings1);
					return (T?)serializer1.Deserialize(xml1);
				}
			});
		}


		/* methods */


		/// <summary>
		/// (async) Асинхронно сериализует объект небольшого или среднего размера в файл.
		/// Сериализация сначала полностью подготавливается в памяти, после чего быстро сбрасывается на диск.
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для сохранения.</param>
		/// <param name="filename">Полный путь к создаваемому файлу.</param>
		/// <param name="encoding">Кодировка текстового файла. Если не задана, используется UTF-8.</param>
		/// <param name="namespaces">
		/// Пользовательские пространства имен XML. Если не заданы, пространства имен опускаются.
		/// </param>
		/// <param name="useFormatted">Признак форматирования XML-текста (отступы и переносы строк).</param>
		public static async Task SaveSoftObjectToXmlFileAsync<T>(
			T? obj,
			string filename,
			Encoding? encoding = null,
			XmlSerializerNamespaces? namespaces = null,
			bool useFormatted = false)
		{
			if (obj is null)
				return;
			var serializer1 = _getCachedSerializer(typeof(T));
			var settings1 = _getWriterSettings(
				useFormatted, encoding, omitXmlDeclaration: false, isAsync: false);
			using var ms1 = new MemoryStream();
			using (var writer1 = XmlWriter.Create(ms1, settings1))
				serializer1.Serialize(writer1, obj, namespaces ?? _emptyNamespaces);
			ms1.Position = 0;
			using var stream1 = new FileStream(
				filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true);
			await ms1.CopyToAsync(stream1);
		}


		/// <summary>
		/// (async) Асинхронно сериализует объект в большой XML-файл, используя потоковую запись
		/// напрямую на диск в фоновом потоке. Рекомендуется для тяжелых объектов во избежание
		/// выделения памяти в куче больших объектов (LOH).
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для сохранения.</param>
		/// <param name="filename">Полный путь к создаваемому файлу.</param>
		/// <param name="encoding">Кодировка текстового файла. Если не задана, используется UTF-8.</param>
		/// <param name="namespaces">Пользовательские пространства имен XML. Если не заданы, пространства имен опускаются.</param>
		/// <param name="useFormatted">Признак форматирования XML-текста (отступы и переносы строк).</param>
		public static async Task SaveHardObjectToXmlFileAsync<T>(
			T? obj,
			string filename,
			Encoding? encoding = null,
			XmlSerializerNamespaces? namespaces = null,
			bool useFormatted = false)
		{
			if (obj is null)
				return;
			var serializer1 = _getCachedSerializer(typeof(T));
			var settings1 = _getWriterSettings(
				useFormatted, encoding, omitXmlDeclaration: false, isAsync: false);
			await Task.Run(() =>
			{
				using var fs1 = new FileStream(
					filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: false);
				using var writer1 = XmlWriter.Create(fs1, settings1);

				serializer1.Serialize(writer1, obj, namespaces ?? _emptyNamespaces);
				writer1.Flush();
			});
		}


		/* privates */


		private static readonly ConcurrentDictionary<(Type Type, string Namespace), XmlSerializer> _serializerCache
			= new();


		private static readonly XmlSerializerNamespaces _emptyNamespaces
			= new([new XmlQualifiedName(string.Empty, string.Empty)]);


		private static XmlWriterSettings _getWriterSettings(
			bool useFormatted,
			Encoding? encoding = null,
			bool omitXmlDeclaration = false,
			bool isAsync = false)
		{
			XmlWriterSettings settings1 = new()
			{
				Async = isAsync,
				OmitXmlDeclaration = omitXmlDeclaration,
				NewLineChars = Environment.NewLine,
			};
			if (encoding is not null)
				settings1.Encoding = encoding;
			if (useFormatted)
			{
				settings1.Indent = true;
				settings1.IndentChars = "  ";
			}
			return settings1;
		}


		private static XmlReaderSettings _getReaderSettings()
		{
			return new XmlReaderSettings
			{
				IgnoreWhitespace = true,
				IgnoreComments = true
			};
		}


		private static XmlSerializer _getCachedSerializer(
			Type type,
			string? defaultNamespace = null)
		{
			string ns1 = defaultNamespace ?? string.Empty;
			return _serializerCache.GetOrAdd(
				(type, ns1), static x => new XmlSerializer(x.Type, x.Namespace));
		}

	}

}
