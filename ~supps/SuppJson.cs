// rev 2026-09-22

using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Вспомогательный класс для высокопроизводительной работы с JSON (классический подход и Source Generation).
	/// </summary>
	public static class SuppJson
	{

		// --- Дополнительная информация ---
		//
		// // Сюда пишем все типы, которые будем превращать в JSON или читать из него.
		// [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default)]
		// [JsonSerializable(typeof(Person))]
		// [JsonSerializable(typeof(List<Person>))] // Если нужно работать со списками
		// public partial class MyProjectJsonContext : JsonSerializerContext { }

		// // Передаем объект и метаданные типа: AppJsonContext.Default.Person
		// string jsonText = SuppJson.GetJsonStringFromObjectGen(originalPerson, MyProjectJsonContext.Default.Person);


		/* consts */


		/// <summary>
		/// Параметры сериализации JSON по умолчанию.
		/// </summary>
		public static readonly JsonSerializerOptions DEFAULT_JSON_SERIALIZER_OPTIONS = new()
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			PropertyNameCaseInsensitive = true,
			//WriteIndented = true, // красивая печать
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};


		/// <summary>
		/// Параметры записи JSON по умолчанию.
		/// </summary>
		public static readonly JsonWriterOptions DEFAULT_JSON_WRITER_OPTIONS = new()
		{
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		};


		/* functions */


		/// <summary>
		/// Десериализует объект из строки JSON.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="json">Исходная строка JSON.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>
		/// Десериализованный объект типа <typeparamref name="T"/>
		/// или значение по умолчанию, если строка пуста.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromJsonString<T>(
			string json,
			JsonSerializerOptions? options = null)
		{
			if (string.IsNullOrEmpty(json))
				return default;
			return JsonSerializer.Deserialize<T>(
				json, options ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// Десериализует объект из массива байт JSON (UTF-8).
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="utf8Json">Исходный массив байт в кодировке UTF-8.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromJsonBytes<T>(
			byte[] utf8Json,
			JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Deserialize<T>(
				utf8Json, options ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// Десериализует объект из readonly-последовательности байт JSON (UTF-8).
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="utf8Json">Входящий срез байт памяти в кодировке UTF-8.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromJsonSpan<T>(
			ReadOnlySpan<byte> utf8Json,
			JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Deserialize<T>(
				utf8Json, options ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// Десериализует объект из синхронного потока данных JSON.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="stream">Входящий поток данных.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromJsonStream<T>(
			Stream stream,
			JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Deserialize<T>(
				stream, options ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// (async) Асинхронно десериализует объект из потока данных JSON.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="stream">Входящий асинхронный поток данных.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>
		/// Задача, представляющая асинхронную операцию десериализации,
		/// с результатом типа <typeparamref name="T"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T?> GetObjectFromJsonStreamAsync<T>(
			Stream stream,
			JsonSerializerOptions? options = null)
		{
			return JsonSerializer.DeserializeAsync<T>(
				stream, options ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// Десериализует объект из файла JSON.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="filename">Полный путь к файлу JSON на диске.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		public static T? GetObjectFromJsonFile<T>(
			string filename,
			JsonSerializerOptions? options = null)
		{
			using var stream1 = new FileStream(
				filename, FileMode.Open, FileAccess.Read);
			return GetObjectFromJsonStream<T>(
				stream1, options);
		}


		/// <summary>
		/// (async) Асинхронно десериализует объект из файла JSON.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="filename">Полный путь к файлу JSON на диске.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>
		/// Задача, содержащая десериализованный объект типа <typeparamref name="T"/>.
		/// </returns>
		public static async Task<T?> GetObjectFromJsonFileAsync<T>(
			string filename,
			JsonSerializerOptions? options = null)
		{
			using var stream1 = new FileStream(
				filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
			return await GetObjectFromJsonStreamAsync<T>(
				stream1, options);
		}


		/// <summary>
		/// Сериализует объект в строку JSON.
		/// </summary>
		/// <param name="obj">Экземпляр объекта для сериализации.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>Строка, содержащая JSON-представление объекта.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetJsonStringFromObject(
			object obj,
			JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Serialize(
				obj, options ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// Сериализует объект в массив байт JSON (UTF-8).
		/// </summary>
		/// <param name="obj">Экземпляр объекта для сериализации.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <returns>Массив байт в кодировке UTF-8.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetJsonBytesFromObject(
			object obj,
			JsonSerializerOptions? options = null)
		{
			return JsonSerializer.SerializeToUtf8Bytes(
				obj, options ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// (Source Gen) Десериализует объект из строки JSON без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="json">Исходная строка JSON.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromJsonStringGen<T>(
			string json,
			JsonTypeInfo<T> jsonTypeInfo)
		{
			return JsonSerializer.Deserialize(
				json, jsonTypeInfo);
		}


		/// <summary>
		/// (Source Gen) Десериализует объект из массива байт JSON (UTF-8) без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="utf8Json">Исходный массив байт в кодировке UTF-8.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromJsonBytesGen<T>(
			byte[] utf8Json,
			JsonTypeInfo<T> jsonTypeInfo)
		{
			return JsonSerializer.Deserialize(
				utf8Json, jsonTypeInfo);
		}


		/// <summary>
		/// (Source Gen) Десериализует объект из readonly-последовательности байт без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="utf8Json">Входящий срез байт памяти в кодировке UTF-8.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <returns>Десериализованный объект типа <typeparamref name="T"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T? GetObjectFromJsonSpanGen<T>(
			ReadOnlySpan<byte> utf8Json,
			JsonTypeInfo<T> jsonTypeInfo)
		{
			return JsonSerializer.Deserialize(
				utf8Json, jsonTypeInfo);
		}


		/// <summary>
		/// (async) (Source Gen) Асинхронно десериализует объект из потока JSON без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="stream">Входящий асинхронный поток данных.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <returns>Задача, содержащая десериализованный объект типа <typeparamref name="T"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ValueTask<T?> GetObjectFromJsonStreamGenAsync<T>(
			Stream stream,
			JsonTypeInfo<T> jsonTypeInfo)
		{
			return JsonSerializer.DeserializeAsync(
				stream, jsonTypeInfo);
		}


		/// <summary>
		/// (async) (Source Gen) Асинхронно десериализует объект из файла JSON без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип результирующего объекта.</typeparam>
		/// <param name="filename">Полный путь к файлу JSON на диске.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <returns>Задача, содержащая десериализованный объект типа <typeparamref name="T"/>.</returns>
		public static async Task<T?> GetObjectFromJsonFileGenAsync<T>(
			string filename,
			JsonTypeInfo<T> jsonTypeInfo)
		{
			using var stream1 = new FileStream(
				filename, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
			return await GetObjectFromJsonStreamGenAsync(
				stream1, jsonTypeInfo);
		}


		/// <summary>
		/// (Source Gen) Сериализует объект в строку JSON без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для сериализации.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <returns>Строка, содержащая JSON-представление объекта.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetJsonStringFromObjectGen<T>(
			T obj,
			JsonTypeInfo<T> jsonTypeInfo)
		{
			return JsonSerializer.Serialize(
				obj, jsonTypeInfo);
		}


		/// <summary>
		/// (Source Gen) Сериализует объект в массив байт JSON (UTF-8) без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для сериализации.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <returns>Массив байт в кодировке UTF-8.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] GetJsonBytesFromObjectGen<T>(
			T obj,
			JsonTypeInfo<T> jsonTypeInfo)
		{
			return JsonSerializer.SerializeToUtf8Bytes(
				obj, jsonTypeInfo);
		}


		/// <summary>
		/// Записывает объект в поток данных JSON.
		/// </summary>
		/// <param name="obj">Экземпляр объекта для записи.</param>
		/// <param name="stream">Целевой поток данных для записи.</param>
		/// <param name="serializerOptions">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <param name="writerOptions">Параметры конфигурации низкоуровневого писателя.</param>
		public static void WriteObjectToJsonStream(
			object obj,
			Stream stream,
			JsonSerializerOptions? serializerOptions = null,
			JsonWriterOptions writerOptions = default)
		{
			var encoder1 = writerOptions.Encoder == null
				? DEFAULT_JSON_WRITER_OPTIONS : writerOptions;
			using var writer1 = new Utf8JsonWriter(
				stream, encoder1);
			JsonSerializer.Serialize(
				writer1, obj, serializerOptions ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
		}


		/// <summary>
		/// (async) Асинхронно записывает объект в поток данных JSON с применением параметров конфигурации записи.
		/// </summary>
		/// <param name="obj">Экземпляр объекта для записи.</param>
		/// <param name="stream">Целевой асинхронный поток для записи.</param>
		/// <param name="serializerOptions">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <param name="writerOptions">
		/// Параметры конфигурации низкоуровневого писателя.
		/// </param>
		/// <returns>Задача, представляющая асинхронную операцию записи.</returns>
		public static async Task WriteObjectToJsonStreamAsync(
			object obj,
			Stream stream,
			JsonSerializerOptions? serializerOptions = null,
			JsonWriterOptions writerOptions = default)
		{
			var encoder1 = writerOptions.Encoder == null
				? DEFAULT_JSON_WRITER_OPTIONS : writerOptions;
			await using var writer1 = new Utf8JsonWriter(
				stream, encoder1);
			JsonSerializer.Serialize(
				writer1, obj, serializerOptions ?? DEFAULT_JSON_SERIALIZER_OPTIONS);
			await writer1.FlushAsync();
		}


		/// <summary>
		/// Сохраняет объект в файл формата JSON.
		/// </summary>
		/// <param name="obj">Экземпляр объекта для сохранения.</param>
		/// <param name="filename">Полный путь к создаваемому файлу на диске.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <param name="writerOptions">Параметры конфигурации низкоуровневого писателя.</param>
		public static void SaveObjectToJsonFile(
			object obj,
			string filename,
			JsonSerializerOptions? options = null,
			JsonWriterOptions writerOptions = default)
		{
			using var stream1 = new FileStream(
				filename, FileMode.Create, FileAccess.Write);
			WriteObjectToJsonStream(
				obj, stream1, options, writerOptions);
		}


		/// <summary>
		/// (async) Асинхронно сохраняет объект в файл формата JSON.
		/// </summary>
		/// <param name="obj">Экземпляр объекта для сохранения.</param>
		/// <param name="filename">Полный путь к создаваемому файлу на диске.</param>
		/// <param name="options">
		/// Параметры сериализации. Если не заданы, используются параметры по умолчанию.
		/// </param>
		/// <param name="writerOptions">Параметры конфигурации низкоуровневого писателя.</param>
		/// <returns>Задача, представляющая асинхронную операцию сохранения.</returns>
		public static async Task SaveObjectToJsonFileAsync(
			object obj,
			string filename,
			JsonSerializerOptions? options = null,
			JsonWriterOptions writerOptions = default)
		{
			using var stream1 = new FileStream(
				filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);
			await WriteObjectToJsonStreamAsync(
				obj, stream1, options, writerOptions);
		}


		/// <summary>
		/// (async) (Source Gen) Асинхронно записывает объект в поток JSON
		/// без использования рантайм-рефлексии с применением параметров конфигурации записи.
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для записи.</param>
		/// <param name="stream">Целевой асинхронный поток для записи.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <param name="writerOptions">Параметры конфигурации низкоуровневого писателя.</param>
		/// <returns>Задача, представляющая асинхронную операцию записи.</returns>
		public static async Task WriteObjectToJsonStreamGenAsync<T>(
			T obj,
			Stream stream,
			JsonTypeInfo<T> jsonTypeInfo,
			JsonWriterOptions writerOptions = default)
		{
			var encoder1 = writerOptions.Encoder == null
				? DEFAULT_JSON_WRITER_OPTIONS : writerOptions;
			await using var writer1 = new Utf8JsonWriter(
				stream, encoder1);
			JsonSerializer.Serialize(
				writer1, obj, jsonTypeInfo);
			await writer1.FlushAsync();
		}


		/// <summary>
		/// (async) (Source Gen) Асинхронно сохраняет объект в файл JSON без использования рантайм-рефлексии.
		/// </summary>
		/// <typeparam name="T">Тип сериализуемого объекта.</typeparam>
		/// <param name="obj">Экземпляр объекта для сохранения.</param>
		/// <param name="filename">Полный путь к создаваемому файлу на диске.</param>
		/// <param name="jsonTypeInfo">Метаданные типа, сгенерированные компилятором.</param>
		/// <param name="writerOptions">Параметры конфигурации низкоуровневого писателя.</param>
		/// <returns>Задача, представляющая асинхронную операцию сохранения.</returns>
		public static async Task SaveObjectToJsonFileGenAsync<T>(
			T obj,
			string filename,
			JsonTypeInfo<T> jsonTypeInfo,
			JsonWriterOptions writerOptions = default)
		{
			using var stream1 = new FileStream(
				filename, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.Asynchronous);
			await WriteObjectToJsonStreamGenAsync(
				obj, stream1, jsonTypeInfo, writerOptions);
		}

	}

}
