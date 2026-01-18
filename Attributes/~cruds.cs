using Ans.Net10.Common.Crud;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Ans.Net10.Common.Attributes
{

	[AttributeUsage(
		AttributeTargets.Property
		| AttributeTargets.Field
		| AttributeTargets.Parameter,
		AllowMultiple = false)]
	public class AnsFieldAttribute(
		CrudFieldTypeEnum type)
		: Attribute
	{
		public CrudFieldTypeEnum CrudType { get; } = type;
	}



	public class AnsRequiredAttribute
		: RequiredAttribute
	{
		public AnsRequiredAttribute()
		{
			ErrorMessage = Resources.Form.Text_ValueIsRequired;
		}
	}



	public class AnsLengthMaxAttribute
		: MaxLengthAttribute
	{
		public AnsLengthMaxAttribute(
			int length)
			: base(length)
		{
			ErrorMessage = string.Format(
				Resources.Form.Template_LengthMaxLimit, length);
		}
	}



	public class AnsLengthMinAttribute
		: MinLengthAttribute
	{
		public AnsLengthMinAttribute(
			int length)
			: base(length)
		{
			ErrorMessage = string.Format(
				Resources.Form.Template_LengthMinLimit, length);
		}
	}



	public class AnsRegexAttribute
		: RegularExpressionAttribute
	{
		public AnsRegexAttribute(
			[StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
			: base(pattern)
		{
			ErrorMessage = Resources.Form.Text_ValueDoesNotFit;
		}
	}

}
