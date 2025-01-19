using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace TrackXpert_API.Data
{
	public class User : IdentityUser
	{
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiryTime { get; set; }
	}
}
