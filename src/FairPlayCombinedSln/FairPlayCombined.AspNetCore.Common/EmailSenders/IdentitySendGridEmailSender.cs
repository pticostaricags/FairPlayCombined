using FairPlayCombined.Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AspNetCore.Common.EmailSenders
{
    public class IdentitySendGridEmailSender(SendGridClient sendGridClient, 
        ILogger<IdentitySendGridEmailSender> logger, string emailFrom) : IEmailSender<ApplicationUser>
    {
        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            try
            {
                logger.LogInformation("Sending Confirmation Link To: {Email}. Link: {Link}", email, confirmationLink);
                SendGridMessage mailMessage = new()
                {
                    From = new(emailFrom),
                    Subject = "FairPlay Confirmation Link",
                    HtmlContent = $"""
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
                mailMessage.AddTo(email);
                var response = 
                await sendGridClient.SendEmailAsync(mailMessage);
                var responseStrong = await response.Body.ReadAsStringAsync();
                logger.LogError("Response: {ResponseMessage}", responseStrong);
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, message: "Message: {Messaage}", ex.Message);
            }
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            try
            {
                logger.LogInformation("Sending Password Reset Code To: {Email}. Link: {ResetCode}", email, resetCode);
                SendGridMessage mailMessage = new()
                {
                    From = new(emailFrom),
                    Subject = "FairPlay Confirmation Link",
                    HtmlContent = $"""
                <h1>
                FairPlay Password Reset Code
                </h1>
                <p>
                Reset code: {resetCode}
                </p>
                """
                };
                mailMessage.AddTo(email);
                await sendGridClient.SendEmailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, message: "Message: {Message}", ex.Message);
            }
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            try
            {
                logger.LogInformation("Sending Password Reset Link To: {Email}. Link: {Link}", email, resetLink);
                SendGridMessage mailMessage = new()
                {
                    From = new(emailFrom),
                    Subject = "FairPlay Password Reset Link",
                    HtmlContent = $"""
                <h1>
                FairPlay Password Reset Link
                </h1>
                <p>
                Your reset link: {resetLink}
                </p>
                """
                };
                mailMessage.AddTo(email);
                await sendGridClient.SendEmailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, message: "Message: {Message}", ex.Message);
            }
        }
    }
}
