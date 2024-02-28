using System.Threading.Tasks;
using milescarrental.Application.Configuration.Emails;

namespace milescarrental.Infrastructure.Emails
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(EmailMessage message)
        {
            // Integration with email service.

            return;
        }
    }
}