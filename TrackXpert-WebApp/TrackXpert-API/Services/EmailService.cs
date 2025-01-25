
using System.Net.Mail;
using System.Net;
using TrackXpert_API.Data;
using System.Net.Http;

namespace TrackXpert_API.Services
{
    public class EmailService : IEmailService
    {
        SmtpClient _smtpClient;
        public EmailService() 
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("Rasmus782@gmail.com", "nlgd xugb tahd dvva"),
                EnableSsl = true
            };
        }
        public async Task SendConfirmationLinkAsync(string confirmationLink, User user)
        {
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

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
