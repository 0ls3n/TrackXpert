using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackXpert_API.Data;

namespace TrackXpert_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;

		public AuthController(UserManager<User> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel model)
		{
			var user = new User { UserName = model.Email, Email = model.Email };
			var result = await _userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				return Ok(new { message = "User registered successfully" });
			}

			return BadRequest(result.Errors);
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginModel model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
			{
				var token = GenerateJwtToken(user);
				return Ok(new { token });
			}

			return Unauthorized();
		}

		private string GenerateJwtToken(IdentityUser user)
		{
		//	var claims = new[]
		//	{
		//	new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
		//	new Claim(JwtRegisteredClaimNames.Email, user.Email),
		//	new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		//};

			var issuer = _configuration["Jwt:Issuer"];
			var audience = _configuration["Jwt:Audience"];
			var key = _configuration["Jwt:Key"];
			var tokenValidity = _configuration.GetValue<int>("Jwt:Validity");
			var tokenExpiryTimestamp = DateTime.UtcNow.AddMinutes(tokenValidity);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(JwtRegisteredClaimNames.Name, user.UserName),
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
				}),
				Expires = tokenExpiryTimestamp,
				Issuer = issuer,
				Audience = audience,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature),
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(securityToken);
		}

		public class RegisterModel
		{
			public string Email { get; set; }
			public string Password { get; set; }
		}

		public class LoginModel
		{
			public string Email { get; set; }
			public string Password { get; set; }
		}
	}
}
