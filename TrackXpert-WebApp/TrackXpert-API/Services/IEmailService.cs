using System.Net.Mail;
using TrackXpert_API.Data;

namespace TrackXpert_API.Services
{
    public interface IEmailService
    {
        public Task SendConfirmationLinkAsync(string confirmationLink, User user);
    }
}
