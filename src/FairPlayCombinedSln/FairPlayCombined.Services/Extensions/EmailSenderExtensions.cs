using FairPlayCombined.Common.EmailSenders;
using FairPlayCombined.Common.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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
            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityMailDevEmailSender>();
        }
    }
}
