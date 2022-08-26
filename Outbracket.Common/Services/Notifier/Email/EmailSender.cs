using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Outbracket.Common.Constants;
using Outbracket.Common.Helpers;
using Outbracket.Common.Services.Notifier.Email.Models;
using Outbracket.Globalization;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Outbracket.Common.Services.Notifier.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;
        private readonly IRazorViewToStringRenderer _razorRenderer;
        
        public EmailSender(EmailSenderOptions options, IRazorViewToStringRenderer razorRenderer)
        {
            _options = options;
            _razorRenderer = razorRenderer;
        }
        public bool Validate(Recipient recipient)
        {
            return Regex.Match(recipient.Address, RegexConstant.Email).Success;
        }

        public async Task<bool> SendRegistrationMessageAsync(Recipient recipient, string link)
        {
            var view = "/Templates/Emails/EmailConfirmation.cshtml";
            var htmlBody = await _razorRenderer.RenderViewToStringAsync(view, 
                new EmailConfirmation{Email = recipient.Address, Link = link, FullName = recipient.FullName});
            var senderResponse = await Execute(Messages.EmailConfirmationSubject.Item2, "", htmlBody, recipient.Address, recipient.FullName);
            return senderResponse.StatusCode == HttpStatusCode.Accepted;
        }

        public async Task<bool> SendRestorePasswordMessageAsync(Recipient recipient, string link)
        {
            var view = "/Templates/Emails/ResetPassword.cshtml";
            var htmlBody = await _razorRenderer.RenderViewToStringAsync(view, 
                new PasswordReset{Link = link, FullName = recipient.FullName});
            var senderResponse = await Execute(Messages.PasswordResetSubject.Item2, "", htmlBody, recipient.Address, recipient.FullName);
            return senderResponse.StatusCode == HttpStatusCode.Accepted;
        }

        private async Task<Response> Execute(
            string subject, 
            string message,
            string htmlContent,
            string email, 
            string fullName)
        {
            var client = new SendGridClient(_options.ApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_options.SenderEmail, _options.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = htmlContent
            };
            msg.AddTo(new EmailAddress(email, fullName));
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);
 
            return await client.SendEmailAsync(msg);
        }
    }
}