using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TrackXpert_API.Data;
using TrackXpert_API.Models;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email!);
            if (user == null)
            {
                return Unauthorized("User not found");
            }

            // Check Password
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password!);

            if (!isPasswordValid)
            {
                return Unauthorized("Invalid credentials");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized("Email not confirmed. Please check your email to confirm your account.");
            }

            var token = JwtTokenGenerator.GenerateToken(user, _configuration);

            return Ok(new { accessToken = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = new User { UserName = registerDto.Email, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password!);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Auth",
                new { userId = user.Id, token = token },
                Request.Scheme);

            Console.WriteLine(confirmationLink);

            // Emailsender service here to send the confirmation link to the users mail account

            return Ok("Registration successful! Please check your email to confirm your account.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                try
                {
                    var identity = await _userManager.ConfirmEmailAsync(user, token);

                    if (identity.Succeeded)
                    {
                        return Ok();
                    }

                    return BadRequest(identity.Errors);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return NotFound("The user is not found");
            }
        }
    }
}
