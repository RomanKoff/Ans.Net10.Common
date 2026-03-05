using System.Net;

namespace Ans.Net10.Common
{

	public class WebGridResult<T>
	{
		public HttpStatusCode StatusCode { get; set; }
		public IEnumerable<T> Grid { get; set; }
	}


	public class WebGridHelper<T>
	{

		private readonly HttpClient _httpClient;
		private readonly string _url;
		private readonly Func<string, T> _parser;


		/* ctor */


		public WebGridHelper(
			HttpClient httpClient,
			string url,
			Func<string, T> parser)
		{
			_httpClient = httpClient;
			_url = url;
			_parser = parser;
		}


		/* functions */


		public virtual WebGridResult<T> SendQuery()
		{
			return _getAsync().Result;
		}


		/* privates */


		private async Task<WebGridResult<T>> _getAsync()
		{
			var res1 = await _httpClient.GetAsync(_url);
			if (res1.IsSuccessStatusCode)
				return new WebGridResult<T>
				{
					StatusCode = res1.StatusCode,
					Grid = SuppGrid.MakeFromStream(_parser, res1.Content.ReadAsStream())
				};
			return new WebGridResult<T>
			{
				StatusCode = res1.StatusCode,
				Grid = default
			};
		}

	}

}
