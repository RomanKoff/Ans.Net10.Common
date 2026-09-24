// rev 2026-09-23

using System.Net;
using System.Net.Http.Headers;

namespace Ans.Net10.Common
{

	/// <summary>
	/// Базовый класс для инкапсуляции результатов веб-запросов и ответов сервера.
	/// </summary>
	/// <typeparam name="T">Тип ожидаемых полезных данных (Content) ответа.</typeparam>
	public class _WebResult_Base<T>
	{

		/// <summary>
		/// Возвращает или задает десериализованные полезные
		/// данные (контент), полученные из ответа сервера.
		/// </summary>
		public T? Content { get; set; }


		/// <summary>
		/// Возвращает или задает HTTP-код статуса ответа от сервера.
		/// </summary>
		public HttpStatusCode StatusCode { get; set; }


		/// <summary>
		/// Возвращает или задает коллекцию заголовков HTTP-ответа от сервера.
		/// </summary>
		public HttpResponseHeaders? Headers { get; set; }


		/// <summary>
		/// Возвращает или задает признак возникновения ошибки при десериализации полученных данных.
		/// </summary>
		public bool IsDeserializationError { get; set; }


		/// <summary>
		/// Возвращает или задает исключение, возникшее в процессе десериализации данных.
		/// </summary>
		public Exception? DeserializationException { get; set; }


		/// <summary>
		/// Возвращает или задает строковое содержимое (тело) ошибки, возвращенное сервером.
		/// </summary>
		public string? ErrorBody { get; set; }

	}

}
