using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
                return Unauthorized("Email or password is incorrect!");
            }

            // Check Password
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password!);

            if (!isPasswordValid)
            {
                return Unauthorized("Email or password is incorrect!");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized("Email not confirmed. Please check your email to confirm your account.");
            }

            var accessToken = JwtTokenGenerator.GenerateToken(user, _configuration);

            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            // Make a check on if the user already exists, if it does then throw an error message
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
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("Rasmus782@gmail.com", "nlgd xugb tahd dvva"),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("Rasmus782@gmail.com"),
                Subject = "Confirmation email",
                Body = @"
                        <html>
                        <body style='background-color: #1c1c1c; color: #ffffff; font-family: Arial, sans-serif; padding: 20px; text-align: center;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px; border-radius: 10px; background-color: #2c2c2c;'>
                                <img src='https://drive.google.com/uc?id=1UzqjX47F9E9DwH6gP25eicWmvWfrc76Y' alt='TrackXpert Logo' style='margin-bottom: 20px; width: 64px; height: 64px;'>
                                <h2 style='color: #ffffff; font-size: 24px; margin: 0;'>Confirm Your Email</h2>
                                <p style='color: #b3b3b3; font-size: 16px; margin: 20px 0;'>Hi there! You're just one step away from accessing your TrackXpert account. Please click the button below to confirm your email address:</p>
                                <a href='" + confirmationLink + @"'
                                   style='display: inline-block; background-color: #8e44ad; color: #ffffff; text-decoration: none; 
                                          padding: 12px 20px; border-radius: 5px; font-size: 16px; font-weight: bold;'>
                                   Confirm Email
                                </a>
                                <p style='color: #757575; font-size: 12px; margin-top: 20px;'>If you didn’t sign up for TrackXpert, you can safely ignore this email.</p>
                            </div>
                        </body>
                        </html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(user.Email!);

            smtpClient.Send(mailMessage);

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
                        var blazorAppUrl = $"https://localhost:7139";
                        return Redirect(blazorAppUrl);
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

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshToken)
        {
            if (refreshToken == null || string.IsNullOrEmpty(refreshToken.RefreshToken))
            {
                return BadRequest("Invalid request");
            }

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken.RefreshToken);
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            var newAccessToken = JwtTokenGenerator.GenerateToken(user, _configuration);

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[128];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
