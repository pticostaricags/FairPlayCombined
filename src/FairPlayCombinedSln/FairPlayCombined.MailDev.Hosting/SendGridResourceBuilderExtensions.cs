//Check: https://learn.microsoft.com/en-us/dotnet/aspire/extensibility/custom-resources?tabs=windows
using Aspire.Hosting.ApplicationModel;

// Put extensions in the Aspire.Hosting namespace to ease discovery as referencing
// the .NET Aspire hosting package automatically adds this namespace.
namespace Aspire.Hosting;

public static class SendGridResourceBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="SendGridResource"/> to the given
    /// <paramref name="builder"/> instance. Uses the "2.0.2" tag.
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">The name of the resource.</param>
    /// <param name="httpPort">The HTTP port.</param>
    /// <param name="smtpPort">The SMTP port.</param>
    /// <returns>
    /// An <see cref="IResourceBuilder{SendGridResource}"/> instance that
    /// represents the added SendGrid resource.
    /// </returns>
    public static IResourceBuilder<SendGridResource> AddSendGrid(
        this IDistributedApplicationBuilder builder,
        string name,
        int? smtpPort = null)
    {
        // The AddResource method is a core API within .NET Aspire and is
        // used by resource developers to wrap a custom resource in an
        // IResourceBuilder<T> instance. Extension methods to customize
        // the resource (if any exist) target the builder interface.
        var resource = new SendGridResource(name);

        return builder.AddResource(resource)
            .WithEnvironment(callback =>
            {
                var smtpServer = builder.Configuration["SMTPServer"];
                var smtpPort = builder.Configuration["SMTPPort"];
                var smtpUsername = builder.Configuration["STMPUsername"];
                var smtpPassword = builder.Configuration["SMTPPassword"];
                callback.EnvironmentVariables.Add("STMPUsername", smtpUsername!);
                callback.EnvironmentVariables.Add("SMTPPassword", smtpPassword!);
                callback.EnvironmentVariables.Add("ConnectionStrings__smtp",
                    $"smtp://{smtpServer}:{smtpPort}");
            })
            .WithEndpoint(
                          port: smtpPort,
                          name: SendGridResource.SmtpEndpointName);
    }
}