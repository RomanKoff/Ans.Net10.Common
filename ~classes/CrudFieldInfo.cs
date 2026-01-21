namespace Ans.Net10.Common
{

	public class CrudFieldInfo
	{
		public string Name { get; set; }
		public CrudFaceHelper Face { get; set; }
		public bool IsRequired { get; set; }
		public string[] Errors { get; set; }

		public bool HasErrors
			=> Errors?.Length > 0;
	}

}
