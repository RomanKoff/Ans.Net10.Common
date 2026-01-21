using Ans.Net10.Common.Resources;
using System.Resources;

namespace Ans.Net10.Common
{

	public class ResourcesHelper
	{

		/* ctor */


		public ResourcesHelper(
			params ResourceManager[] resources)
		{
			Resources = resources.GetArrayAdd(Faces.ResourceManager);
		}


		/* readonly properties */


		public ResourceManager[] Resources { get; }


		/* functions */


		public CrudFaceHelper GetCalcFaceHelper(
			string key)
		{
			if (key == null)
				return null;
			var face1 = new CrudFaceHelper(key, key);
			if (Resources == null)
				return face1;
			foreach (var resource1 in Resources)
			{
				var s1 = resource1?.GetString(key);
				if (!string.IsNullOrEmpty(s1))
				{
					var face2 = new CrudFaceHelper(key, s1);
					if (face2.HasTitle)
						face1.Title = face2.Title;
					if (face2.HasShortTitle)
						face1.ShortTitle = face2.ShortTitle;
					if (face2.HasDescription)
						face1.Description = face2.Description;
					if (face2.HasSample)
						face1.Sample = face2.Sample;
					if (face2.HasHelpLink)
						face1.HelpLink = face2.HelpLink;
				}
			}
			return face1;
		}


		public CrudFaceHelper GetFaceHelper(
			string name)
		{
			return new CrudFaceHelper(name, GetCalcFaceHelper(name).GetFace());
		}

	}

}
