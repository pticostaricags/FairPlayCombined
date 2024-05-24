using FairPlayCombined.Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace FairPlayCombined.Common.EmailSenders
{
    public class IdentityMailDevEmailSender(
        SmtpClient smtpClient,
        ILogger<IdentityMailDevEmailSender> logger) : IEmailSender<ApplicationUser>
    {
        private const string EMAIL_FROM = "test@test.test";
        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            logger.LogInformation("Sending Confirmation Link To: {Email}. Link: {Link}", email, confirmationLink);
            MailMessage mailMessage = new()
            {
                From = new(EMAIL_FROM),
                Subject = "FairPlay Confirmation Link",
                IsBodyHtml = true,
                Body = $"""
                <h1>
                FairPlay Confirmation Link
                </h1>
                <p>
                <a href="{confirmationLink}">
                Your confirmation link
                </a>
                </p>
                """
            };
            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            logger.LogInformation("Sending Password Reset Code To: {Email}. Link: {ResetCode}", email, resetCode);
            MailMessage mailMessage = new()
            {
                From = new(EMAIL_FROM),
                Subject = "FairPlay Confirmation Link",
                IsBodyHtml = true,
                Body = $"""
                <h1>
                FairPlay Password Reset Code
                </h1>
                <p>
                Reset code: {resetCode}
                </p>
                """
            };
            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            logger.LogInformation("Sending Password Reset Link To: {Email}. Link: {Link}", email, resetLink);
            MailMessage mailMessage = new()
            {
                From = new(EMAIL_FROM),
                Subject = "FairPlay Password Reset Link",
                IsBodyHtml = true,
                Body = $"""
                <h1>
                FairPlay Password Reset Link
                </h1>
                <p>
                Your reset link: {resetLink}
                </p>
                """
            };
            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
