using FairPlayCombined.Models.GoogleAuth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.SharedAuth.Extensions
{
    public static class AuthExtensions
    {
        public static AuthenticationBuilder AddGoogleAuth(this AuthenticationBuilder builder,
            GoogleAuthClientSecretInfo googleAuthClientSecretInfo, string[] scopes,
            bool saveTokens)
        {
            builder.AddGoogle(options =>
            {
                options.ClientId = googleAuthClientSecretInfo.installed!.client_id!;
                options.ClientSecret = googleAuthClientSecretInfo.installed.client_secret!;
                foreach (var scope in scopes)
                {
                    options.Scope.Add(scope);
                }
                options.SaveTokens = saveTokens;
                options.Events.OnRedirectToAuthorizationEndpoint = context =>
                {
                    context.Response.Redirect(context.RedirectUri + $"&hl={CultureInfo.CurrentUICulture.Name}");
                    return Task.CompletedTask;
                };
            });
            return builder;
        }
    }
}
