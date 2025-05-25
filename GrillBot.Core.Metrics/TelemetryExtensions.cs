using GrillBot.Core.Metrics.CustomTelemetry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;

namespace GrillBot.Core.Metrics;

public static class TelemetryExtensions
{
    private const string METER_NAME = "GrillBot-ServiceMeter";

    public static IHostApplicationBuilder AddTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(opt =>
        {
            opt.IncludeFormattedMessage = true;
            opt.IncludeScopes = true;

            var resourceBuilder = ResourceBuilder.CreateDefault();
            resourceBuilder.AddService(builder.Environment.ApplicationName);
            opt.SetResourceBuilder(resourceBuilder);

            opt.AddOtlpExporter();
        });

        builder.Services.AddHttpLogging(c => c.LoggingFields = HttpLoggingFields.All);

        builder.Services.Configure<AspNetCoreTraceInstrumentationOptions>(
            opt => opt.Filter = ctx => ctx.Request.Path != "/metrics" && ctx.Request.Path != "/health"
        );

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(b => b.AddService(builder.Environment.ApplicationName))
            .WithTracing(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddSource("*")
                .AddQuartzInstrumentation()
                .AddOtlpExporter()
            )
            .WithMetrics(b => b
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation()
                .AddProcessInstrumentation()
                .AddPrometheusExporter()
                .AddMeter(METER_NAME)
            );

        builder.Services.AddSingleton(
            provider => provider.GetRequiredService<IMeterFactory>().Create(METER_NAME)
        );

        return builder;
    }

    public static void UseTelemetry(this IApplicationBuilder app)
    {
        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.UseHttpLogging();
    }

    public static IServiceCollection AddCustomTelemetryBuilder<TBuilder>(this IServiceCollection services) where TBuilder : class, ICustomTelemetryBuilder
        => services.AddSingleton<ICustomTelemetryBuilder, TBuilder>();
}
