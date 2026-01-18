namespace Ans.Net10.Common
{

	public static partial class _Consts
	{

		public static readonly Func<char, char> FuncSelf
			= new(x => x);

		public static readonly Func<char, char> FuncToLower
			= new(char.ToLower);

		public static readonly Func<char, char> FuncToUpper
			= new(char.ToUpper);

	}

}
