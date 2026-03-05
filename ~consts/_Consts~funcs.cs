namespace Ans.Net10.Common
{

	public static partial class _Consts
	{

		public static readonly Func<char, char> FuncSelf
			= x => x;

		public static readonly Func<char, char> FuncToLower
			= char.ToLower;

		public static readonly Func<char, char> FuncToUpper
			= char.ToUpper;

	}

}
