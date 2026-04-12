namespace Ans.Net10.Common
{

	public class TagStyler
	{

		/* ctors */


		public TagStyler(
			TagClassesBuilder classes,
			TagStylesBuilder styles = null)
		{
			Classes = classes ?? new();
			Styles = styles ?? new();
		}


		public TagStyler(
			string cssClasses,
			string cssStyles = null)
			: this(
				 new TagClassesBuilder(cssClasses),
				 new TagStylesBuilder(cssStyles))
		{
		}


		public TagStyler()
		{
			Classes = new();
			Styles = new();
		}


		/* properties */


		public TagClassesBuilder Classes { get; set; }
		public TagStylesBuilder Styles { get; set; }


		/* methods */


		public void ApplyBase(
			TagStyler styler)
		{
			Classes.ApplyOriginal(styler.Classes.Items);
			Styles.ApplyOriginal(styler.Styles.Items);
		}

	}

}
