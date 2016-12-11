using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace PictureTagger
{
	public static class AuthenticationExtensions
	{
		public static IEnumerable<Claim> GetClaims(this IPrincipal user)
		{
			return (user.Identity as ClaimsIdentity)?.Claims;
		}
	}
}