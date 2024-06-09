//Sample code based on : https://youtu.be/jVILDZtuUrI
using System.Diagnostics;
using System.Net.Mail;

namespace FairPlayCombined.Common.Smtp
{
    public class SmtpTelemetry
    {
        public const string ActivitySourceName = "Smtp";
        public ActivitySource ActivitySource { get; set; } = new(ActivitySourceName);
    }

    public interface ISmtpClient
    {
        Task SendMailAsync(MailMessage mailMessage);
    }

    public class SmtpClientWithTelemetry(SmtpClient smtpClient, SmtpTelemetry smtpTelemetry) : ISmtpClient
    {
        public async Task SendMailAsync(MailMessage mailMessage)
        {
            var activity = smtpTelemetry.ActivitySource.StartActivity(name: nameof(SendMailAsync), ActivityKind.Client);
            if (activity is not null)
            {
                activity.AddTag("mail.from", mailMessage.From);
                activity.AddTag("mail.to", mailMessage.To);
                activity.AddTag("mail.subject", mailMessage.Subject);
                activity.AddTag("peer.service", $"{smtpClient.Host}:{smtpClient.Port}");
            }
            try
            {
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                if (activity is not null)
                {
                    activity.AddTag("exception.message", ex.Message);
                    activity.AddTag("exception.stackTrace", ex.StackTrace);
                    activity.AddTag("exception.type", ex.GetType().FullName);
                }
                throw;
            }
            finally
            {
                activity?.Stop();
            }
        }
    }
}
