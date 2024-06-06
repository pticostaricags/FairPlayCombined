using FairPlayCombined.AspNetCore.Common.EmailSenders;
using FairPlayCombined.Common.EmailSenders;
using FairPlayCombined.Common.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Extensions
{
    public static class EmailSenderExtensions
    {
        public static void AddIdentityEmailSender(this WebApplicationBuilder builder)
        {
            if (Convert.ToBoolean(builder.Configuration["UseSendGrid"]))
            {
                builder.Services.AddTransient<SendGridClient>(sp => 
                {
                    var apiKey = builder.Configuration["SMTPPassword"];
                    SendGridClient sendGridClient = new(apiKey: apiKey);
                    return sendGridClient;
                });
                builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentitySendGridEmailSender>(sp => 
                {
                    SendGridClient sendGridClient = sp.GetRequiredService<SendGridClient>();
                    ILogger<IdentitySendGridEmailSender> logger = 
                    sp.GetRequiredService<ILogger<IdentitySendGridEmailSender>>();
                    string emailFrom = builder.Configuration["EmailFrom"]!;
                    IdentitySendGridEmailSender identitySendGridEmailSender = 
                    new(sendGridClient,logger, emailFrom);
                    return identitySendGridEmailSender;
                });
            }
            else
            {
                builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityMailDevEmailSender>();
            }
        }
    }
}
