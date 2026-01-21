using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Ans.Net10.Common
{

	public interface IWebApiHelper<T>
	{
		string BaseUrl { get; }
		JsonSerializerOptions JsonOptions { get; }
		ParamsBuilder Params { get; }

		WebApiResult<T> SendQuery();
		WebApiResult<T> SendQuery(string queryString);
	}



	public class WebApiResult<T>
	{
		public HttpStatusCode StatusCode { get; set; }
		public T Content { get; set; }
	}



	public class WebApiHelper<T>
		: IWebApiHelper<T>
	{

		private readonly HttpClient _httpClient;


		/* ctor */


		public WebApiHelper(
			HttpClient httpClient,
			string baseUrl,
			JsonSerializerOptions jsonOptions = null)
		{
			_httpClient = httpClient;
			BaseUrl = baseUrl;
			JsonOptions = jsonOptions;
		}


		/* readonly properties */


		public string BaseUrl { get; }
		public JsonSerializerOptions JsonOptions { get; }
		public ParamsBuilder Params { get; } = new();


		/* functions */


		public virtual WebApiResult<T> SendQuery(
			string queryString)
		{
			return _getAsync(queryString).Result;
		}


		public WebApiResult<T> SendQuery()
		{
			return SendQuery(Params.ToString());
		}


		/* privates */


		private async Task<WebApiResult<T>> _getAsync(
			string queryString)
		{
			var res1 = await _httpClient.GetAsync($"{BaseUrl}{queryString}");
			if (res1.IsSuccessStatusCode)
				return new WebApiResult<T>
				{
					StatusCode = res1.StatusCode,
					Content = res1.Content.ReadFromJsonAsync<T>(JsonOptions).Result
				};
			return new WebApiResult<T>
			{
				StatusCode = res1.StatusCode,
				Content = default
			};
		}

	}

}
