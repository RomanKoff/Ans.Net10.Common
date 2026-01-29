namespace Ans.Net10.Common.Crud
{

	public enum CrudEntityTypeEnum
	{
		Normal,
		Tree,
		Ordered,
	}



	public enum CrudEntityAfterAddEnum
	{
		List,
		Edit
	}



	public enum CrudManyrefViewEnum
	{
		OnlyThis,
		OnlyTarget,
		All,
	}



	public enum CrudFieldTypeEnum
	{
		Text50, Text100, Text250, Text400,
		Memo, Name, Varname, Email,
		Int, Long, Float, Double, Decimal,
		DateTime, DateOnly, TimeOnly,
		Bool, Enum, Set,
		Reference,
	}



	public enum CrudFieldModeEnum
	{
		Normal,
		Required,
		Unique,
		AbsoluteUnique
	}



	public enum CrudFieldRegistryModeEnum
	{
		Auto,
		Inputs,
		Select
	}

}
