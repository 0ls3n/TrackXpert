using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TrackXpert_API.Data
{
	public class User : IdentityUser
	{
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiryTime { get; set; }

		[Required]
		public string? Firstname { get; set; }
		[Required]
		public string? Lastname { get; set; }
		[Required]
		public string? Displayname { get; set; }
	}
}
