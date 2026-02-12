using System.Data;
using System.Security.Claims;

namespace Ans.Net10.Common
{

	public static partial class _e_Claims
	{

		/* methods */


		public static void AddClaim(
			this ClaimsIdentity identity,
			string type,
			string value)
		{
			if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(value))
				return;
			if (!identity.HasClaim(
				x => x.Type == type && x.Value == value))
			{
				identity.AddClaim(new Claim(type, value));
			}
		}


		public static void AddClaims(
			this ClaimsPrincipal principal,
			string type,
			params string[] values)
		{
			var identity1 = new ClaimsIdentity();
			if (values?.Length > 0)
				foreach (var item1 in values)
					identity1.AddClaim(type, item1);
			principal.AddIdentity(identity1);
		}


		public static void AddRolesClaims(
			this ClaimsPrincipal principal,
			string[] roles)
		{
			principal.AddClaims(ClaimTypes.Role, roles);
		}


		/* functions */


		public static string GetNameIdentifierFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims.FirstOrDefault(
				x => x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
		}


		public static string GetIdUsernameFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims.FirstOrDefault(
				x => x.Type.Equals("id_username"))?.Value;
		}


		public static string GetEmailFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims.FirstOrDefault(
				x => x.Type.Equals(ClaimTypes.Email))?.Value;
		}


		public static string GetNameFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims.FirstOrDefault(
				x => x.Type.Equals("name"))?.Value;
		}


		public static string GetSurnameFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims.FirstOrDefault(
				x => x.Type.Equals(ClaimTypes.Surname))?.Value;
		}


		public static string GetGivenNameFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims.FirstOrDefault(
				x => x.Type.Equals(ClaimTypes.GivenName))?.Value;
		}


		public static string GetGenderFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims.FirstOrDefault(
				x => x.Type.Equals(ClaimTypes.Gender))?.Value;
		}


		public static IEnumerable<string> GetRolesFromClaim(
			this ClaimsPrincipal principal)
		{
			return principal?.Claims
				.Where(x => x.Type == ClaimTypes.Role)
				.Select(x => x.Value);
		}


		//public static IEnumerable<string> GetClaims(
		//	this ClaimsPrincipal principal,
		//	string typePrefix)
		//{
		//	return principal?.Claims
		//		.Where(x => x.Type.StartsWith(typePrefix))
		//		.Select(x => $"{x.Type} {x.Value}");
		//}

	}

}
