using FairPlayCombined.Common.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Mail;

namespace FairPlayCombined.Services.Extensions
{
    public static class SmtpExtensions
    {
        public static void AddSmtpClient(this IHostApplicationBuilder builder, string name)
        {
            builder.Services.AddSingleton<ISmtpClient, SmtpClientWithTelemetry>();
            builder.Services.AddSingleton<SmtpClient>(sp =>
            {
                var smtpUri = new Uri(builder.Configuration.GetConnectionString(name)!);

                var smtpUsername = builder.Configuration["SMTPUsername"];
                var smtpPassword = builder.Configuration["SMTPPassword"];
                var useSSLForSMTP = Convert.ToBoolean(builder.Configuration["UseSSLForSMTP"]);

                var smtpClient = new SmtpClient(smtpUri.Host, smtpUri.Port)
                {
                    EnableSsl = useSSLForSMTP,
                };
                if (!String.IsNullOrWhiteSpace(smtpUsername) && !String.IsNullOrWhiteSpace(smtpPassword))
                {
                    smtpClient.Credentials = new NetworkCredential(userName: smtpUsername, password: smtpPassword);
                }
                return smtpClient;
            });
            builder.Services.AddOpenTelemetry()
                .WithTracing(t => t.AddSource(SmtpTelemetry.ActivitySourceName));
            builder.Services.AddSingleton<SmtpTelemetry>();
        }
    }
}
