using FairPlayCombined.Common.Smtp;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Extensions
{
    public static class SmtpExtensions
    {
        public static void AddSmtpClient(this WebApplicationBuilder builder, string name)
        {
            builder.Services.AddSingleton<ISmtpClient, SmtpClientWithTelemetry>();
            builder.Services.AddSingleton<SmtpClient>(sp =>
            {
                var smtpUri = new Uri(builder.Configuration.GetConnectionString(name)!);

                var smtpClient = new SmtpClient(smtpUri.Host, smtpUri.Port)
                {
                    EnableSsl = !builder.Environment.IsDevelopment()
                };

                return smtpClient;
            });
            builder.Services.AddOpenTelemetry()
                .WithTracing(t => t.AddSource(SmtpTelemetry.ActivitySourceName));
            builder.Services.AddSingleton<SmtpTelemetry>();
        }
    }
}
