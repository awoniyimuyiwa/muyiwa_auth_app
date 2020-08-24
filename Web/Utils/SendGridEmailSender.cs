using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Web.Utils
{
    class SendGridEmailSender : IEmailSender
    {
        readonly SendGridOptions SendGridOptions;

        public SendGridEmailSender(IOptions<SendGridOptions> options)
        {
            SendGridOptions = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        Task Execute(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(SendGridOptions.AppSendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(SendGridOptions.AppFromEmail, SendGridOptions.AppSendGridUser),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }

    class SendGridOptions
    {
        public string AppFromEmail { get; set; }
        public string AppSendGridKey { get; set; }
        public string AppSendGridUser { get; set; }
    }
}
