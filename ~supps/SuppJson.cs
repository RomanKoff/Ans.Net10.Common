using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ans.Net10.Common
{

	public static class SuppJson
	{

		/* readonly properties */


		public static JsonSerializerOptions DefaultJsonSerializerOptions
			=> new()
			{
				DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
				PropertyNameCaseInsensitive = false,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
			};


		public static JsonWriterOptions DefaultJsonWriterOptions
			=> new()
			{
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};


		/* functions */


		/// <summary>
		/// Возвращает объект типа T из строки json
		/// </summary>
		public static T GetObjectFromJsonString<T>(
			string json,
			JsonSerializerOptions options = null)
		{
			return JsonSerializer.Deserialize<T>(
				json,
				options ?? DefaultJsonSerializerOptions);
		}


		/// <summary>
		/// Возвращает объект типа T из потока json
		/// </summary>
		public static T GetObjectFromJson<T>(
			Stream stream,
			JsonSerializerOptions options = null)
		{
			var res1 = JsonSerializer.Deserialize<T>(
				stream,
				options ?? DefaultJsonSerializerOptions);
			return res1;
		}


		/// <summary>
		/// (async) Возвращает объект типа T из потока json 
		/// </summary>
		public static async Task<T> GetObjectFromJsonAsync<T>(
			Stream stream,
			JsonSerializerOptions options = null)
		{
			var res1 = await JsonSerializer.DeserializeAsync<T>(
				stream,
				options ?? DefaultJsonSerializerOptions);
			return res1;
		}


		/// <summary>
		/// Возвращает объект типа T из файла json
		/// </summary>
		public static T GetObjectFromJsonFile<T>(
			string filename,
			JsonSerializerOptions options = null)
		{
			using var stream1 = new FileStream(
				filename,
				FileMode.Open,
				FileAccess.Read);
			return GetObjectFromJson<T>(
				stream1,
				options);
		}


		/// <summary>
		/// Возвращает строку json из объекта
		/// </summary>
		public static string GetJsonStringFromObject(
			object obj,
			JsonSerializerOptions options = null)
		{
			return JsonSerializer.Serialize(
				obj,
				options ?? DefaultJsonSerializerOptions);
		}


		/* methods */


		/// <summary>
		/// Записывает объект в поток json
		/// </summary>
		public static void WriteObjectToStreamJson(
			object obj,
			Stream stream,
			JsonSerializerOptions serializerOptions = null)
		{
			using var writer1 = new Utf8JsonWriter(
				stream,
				DefaultJsonWriterOptions);
			JsonSerializer.Serialize(
				writer1,
				obj,
				serializerOptions ?? DefaultJsonSerializerOptions);
		}


		/// <summary>
		/// Сохраняет объект в файл json
		/// </summary>
		public static void SaveObjectToJsonFile(
			object obj,
			string filename,
			JsonSerializerOptions options = null)
		{
			using var stream1 = new FileStream(
				filename,
				FileMode.Create,
				FileAccess.Write);
			WriteObjectToStreamJson(
				obj,
				stream1,
				options);
		}

	}

}
