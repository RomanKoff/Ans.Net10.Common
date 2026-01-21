using System.Text;

namespace Ans.Net10.Common
{

	public interface ICrudFace
	{
		string Name { get; set; }
		string Title { get; set; }

		string ShortTitle { get; }
		string Description { get; }
		string Sample { get; }
		string HelpLink { get; }
	}



	public class CrudFaceHelper
		: ICrudFace
	{

		/* ctors */


		/// <param name="face">
		/// "title|shortTitle|description|sample|helpLink"
		/// </param>
		public CrudFaceHelper(
			string name,
			string face)
		{
			var a1 = new StringParser(face);
			_init(name, a1.Get(0), a1.Get(1), a1.Get(2), a1.Get(3), a1.Get(4));
		}


		public CrudFaceHelper(
			string name,
			string title,
			string shortTitle,
			string description,
			string sample,
			string helpLink)
		{
			_init(name, title, shortTitle, description, sample, helpLink);
		}


		/* properties */


		public string Name { get; set; }
		public string Title { get; set; }
		public string ShortTitle { get; set; }
		public string Description { get; set; }
		public string Sample { get; set; }
		public string HelpLink { get; set; }


		/* readonly properties */


		public string TitleCalc
			=> HasTitle ? Title : Name;

		public string ShortTitleCalc
			=> HasShortTitle ? ShortTitle : TitleCalc;


		public bool HasTitle
			=> !string.IsNullOrEmpty(Title);


		public bool HasShortTitle
			=> !string.IsNullOrEmpty(ShortTitle);


		public bool HasDescription
			=> !string.IsNullOrEmpty(Description);


		public bool HasSample
			=> !string.IsNullOrEmpty(Sample);


		public bool HasHelpLink
			=> !string.IsNullOrEmpty(HelpLink);


		public bool HasFace
			=> HasTitle || HasShortTitle || HasDescription || HasSample || HasHelpLink;



		/* functions */


		public string GetFace()
		{
			var sb1 = new StringBuilder(Title);
			sb1.Append($"|{ShortTitle}");
			sb1.Append($"|{Description}");
			sb1.Append($"|{Sample}");
			sb1.Append($"|{HelpLink}");
			return sb1.ToString();
		}


		/* privates */


		private void _init(
			string name,
			string title,
			string shortTitle,
			string description,
			string sample,
			string helpLink)
		{
			Name = name;
			Title = title;
			ShortTitle = shortTitle;
			Description = description;
			Sample = sample;
			HelpLink = helpLink;
		}

	}

}
