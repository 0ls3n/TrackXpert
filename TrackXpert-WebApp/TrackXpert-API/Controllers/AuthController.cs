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
using TrackXpert_API.Services;

namespace TrackXpert_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            /* Operation Contract (Login User)
             * Input: username/email(string), password(string)
             * Output: AccessToken Bearer (string)
             * 
             * Pre condition: User must exist in the database, and the user´s email must be confirmed.
             * The password must be the same as when the user registered
             * 
             * Post condition: An accessToken was sent with the response to the user
             */

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

            var refreshToken = JwtTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new { accessToken, refreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            /* Operation Contract (Register User)
             * Input: username/email(string), password(string), firstName(string), lastName(string)
             * Output: Success message, and send an mail to the user with the confirm link
             * 
             * Pre condition: User must not exist in the database already
             * 
             * Post condition: A user instance was created and saved to the database
             */

            if (await _userManager.Users.SingleOrDefaultAsync(x=> x.Displayname == registerDto.Displayname) != null)
            {
                return BadRequest("A user already has that displayname");
            }

            if (await _userManager.FindByEmailAsync(registerDto.Email!) != null)
            {
                return BadRequest("A user is already registered with that email");
            }

            var user = new User 
            { 
                UserName = registerDto.Email,
                Email = registerDto.Email, 
                Firstname = registerDto.Firstname,
                Lastname = registerDto.Lastname,
                Displayname = registerDto.Displayname
            };

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
            await _emailService.SendConfirmationLinkAsync(confirmationLink!, user);

            return Ok("Registration successful! Please check your email to confirm your account.");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            /* Operation Contract (Confirm User)
             * Input: userId(string), token(string)
             * Output: Success message, and redirect to web app confirm success site
             * 
             * Pre condition: User must exist in the database already, and user must not be confirmed already
             * 
             * Post condition: A user's email was confirmed and saved to the database (attribute modification)
             */

            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                try
                {
                    var identity = await _userManager.ConfirmEmailAsync(user, token);

                    if (identity.Succeeded)
                    {
                        var blazorAppUrl = $"https://localhost:7139/auth/emailconfirmedsuccess";
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

        [HttpGet("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmationLink(string email)
        {
            /* Operation Contract (Resend confirmation to user)
             * Input: email(string)
             * Output: Success message, and send mail to the user with confirmation link
             * 
             * Pre condition: User must exist in the database already, and user must not be confirmed already
             * 
             * Post condition: none
             */

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User is not found");
            }

            if (user.EmailConfirmed == true)
            {
                return BadRequest("Email is already confirmed");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = Url.Action(
                "ConfirmEmail",
                "Auth",
                new { userId = user.Id, token = token },
                Request.Scheme);

            await _emailService.SendConfirmationLinkAsync(confirmationLink!, user);

            return Ok("Registration successful! Please check your email to confirm your account.");

        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshToken)
        {
            /* Operation Contract (Refresh user authorization)
             * Input: refreshToken(string)
             * Output: AccessToken Bearer (string)
             * 
             * Pre condition: User must exist in the database, and the user´s email must be confirmed.
             * The refreshToken must be valid
             * 
             * Post condition: An accessToken was sent with the response to the user, and a new refreshToken
             * will be generated and saved to the database
             */

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

            var newRefreshToken = JwtTokenGenerator.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return Ok(new { accessToken = newAccessToken, refreshToken = newRefreshToken });
        }
    }
}
