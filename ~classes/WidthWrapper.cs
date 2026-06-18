namespace Ans.Net10.Common
{

	public enum WidthsEnum
	{
		Full,
		ExtraLarge,
		Large,
		Medium,
		Small,
		ExtraSmall,
		Nothing
	}



	public class WidthWrapper(
		string html,
		WidthsEnum width,
		string cssClass)
	{

		/* readonly properties */


		public string Html { get; } = html;
		public WidthsEnum Width { get; } = width;
		public string CssClass { get; } = cssClass;

		public string AutoStyle
			=> field ??= Width switch
			{
				WidthsEnum.Full => "width:100%",
				WidthsEnum.ExtraLarge => "width:40rem",
				WidthsEnum.Large => "width:30rem",
				WidthsEnum.Medium => "width:20rem",
				WidthsEnum.Small => "width:15rem",
				WidthsEnum.ExtraSmall => "width:10rem",
				_ => "width:6rem"
			};


		/* functions */


		public override string ToString()
		{
			return $"<div class=\"{CssClass}\" style=\"{AutoStyle}\">{Html}</div>";
		}

	}

}
