using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.Data.SqlClient;
using FairPlayCombined.Common.CustomExceptions;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Extensions.Hosting;


public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.ConfigureOpenTelemetry();

        builder.AddDefaultHealthChecks();

        builder.Services.AddServiceDiscovery();

        builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            http.UseServiceDiscovery();
        });
        builder.Services.AddMemoryCache();
        return builder;
    }

    public static void EnhanceConnectionString(string appName, ref string connectionString)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        connectionStringBuilder.ApplicationName = appName;
        connectionString = connectionStringBuilder.ConnectionString;
    }

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddRuntimeInstrumentation()
                       .AddBuiltInMeters();
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    // We want to view all traces in development
                    tracing.SetSampler(new AlwaysOnSampler());
                }

                tracing.AddAspNetCoreInstrumentation()
                       .AddGrpcClientInstrumentation()
                       .AddHttpClientInstrumentation();
            });

        builder.AddOpenTelemetryExporters();

        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }

        // Uncomment the following lines to enable the Prometheus exporter (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
        // builder.Services.AddOpenTelemetry()
        //    .WithMetrics(metrics => metrics.AddPrometheusExporter());

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.Exporter package)
        // builder.Services.AddOpenTelemetry()
        //    .UseAzureMonitor();

        return builder;
    }

    public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            // Add a default liveness check to ensure app is responsive
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }

    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        // Uncomment the following line to enable the Prometheus endpoint (requires the OpenTelemetry.Exporter.Prometheus.AspNetCore package)
        // app.MapPrometheusScrapingEndpoint();

        // All health checks must pass for app to be considered ready to accept traffic after starting
        app.MapHealthChecks("/health");

        // Only health checks tagged with the "live" tag must pass for app to be considered alive
        app.MapHealthChecks("/alive", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("live")
        });
        return app;
    }

    private static MeterProviderBuilder AddBuiltInMeters(this MeterProviderBuilder meterProviderBuilder) =>
        meterProviderBuilder.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "System.Net.Http");

    #region FairPlay Apps Customization

    public static WebApplication UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(configure =>
        {
            configure.Run(async context =>
            {
                var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();
                var error = exceptionHandlerPathFeature?.Error;
                if (error != null)
                {
                    long? errorId = default;
                    try
                    {
                        FairPlayCombinedDbContext fairPlayCombinedDbContext =
                        context.RequestServices.GetRequiredService<FairPlayCombinedDbContext>();
                        ErrorLog errorLog = new()
                        {
                            FullException = error.ToString(),
                            StackTrace = error.StackTrace,
                            Message = error.Message
                        };
                        await fairPlayCombinedDbContext.ErrorLog.AddAsync(errorLog);
                        await fairPlayCombinedDbContext.SaveChangesAsync();
                        errorId = errorLog.ErrorLogId;
                    }
                    catch (Exception)
                    {
                        //Global exception, not rethrowing so server app does not crash
                    }
                    ProblemDetails problemDetails = new ProblemDetails();
                    if (error is RuleException || error is ValidationException)
                    {
                        problemDetails.Detail = error.Message;
                    }
                    else
                    {
                        string userVisibleError = "An error ocurred.";
                        if (errorId.GetValueOrDefault(0) > 0)
                        {
                            userVisibleError += $" Error code: {errorId}";
                        }
                        problemDetails.Detail = userVisibleError;
                    }
                    problemDetails.Status = (int)System.Net.HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync<ProblemDetails>(problemDetails);
                }
            });
        });
        return app;
    }
    #endregion
}
