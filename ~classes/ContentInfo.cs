using System.Net.Http.Headers;

namespace Ans.Net10.Common
{

	public enum ContentGroupEnum
	{
		Archive,
		Audio,
		Bin,
		Code,
		Document,
		Image,
		Text,
		Video
	}



	public class ContentInfo(
		string extention,
		string contentType,
		ContentGroupEnum group,
		bool isWebImage = false,
		bool isJpeg = false)
	{

		public string Extention { get; } = extention;
		public string ContentType { get; } = contentType;
		public ContentGroupEnum Group { get; } = group;
		public bool IsImage { get; } = group == ContentGroupEnum.Image;
		public bool IsWebImage { get; } = isWebImage;
		public bool IsJpeg { get; } = isJpeg;

		public MediaTypeHeaderValue MediaType
			=> field ??= new MediaTypeHeaderValue(ContentType);

	}

}
