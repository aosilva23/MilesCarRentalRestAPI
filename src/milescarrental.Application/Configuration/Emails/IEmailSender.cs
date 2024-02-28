using System.Threading.Tasks;

namespace milescarrental.Application.Configuration.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}