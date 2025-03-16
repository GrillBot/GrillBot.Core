using GrillBot.Core.Services.AuditLog;
using GrillBot.Core.Services.Common;
using GrillBot.Core.Services.Common.Attributes;
using GrillBot.Core.Services.Common.Exceptions;
using GrillBot.Core.Services.Common.Executor;
using GrillBot.Core.Services.Emote;
using GrillBot.Core.Services.Graphics;
using GrillBot.Core.Services.ImageProcessing;
using GrillBot.Core.Services.PointsService;
using GrillBot.Core.Services.RemindService;
using GrillBot.Core.Services.RubbergodService;
using GrillBot.Core.Services.SearchingService;
using GrillBot.Core.Services.UserMeasures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Refit;
using System.Net;
using System.Net.Http.Json;
using System.Reflection;

namespace GrillBot.Core.Services;

public static class ServicesExtensions
{
    public static void RegisterService<TInterface>(this IServiceCollection services, IConfiguration configuration)
        where TInterface : class, IServiceClient
    {
        var serviceName = typeof(TInterface).GetCustomAttribute<ServiceAttribute>()?.ServiceName ??
            throw new ArgumentException("Missing service attribute in the client interface.");

        var isThirdParty = configuration.GetValue<bool>($"Services:{serviceName}:IsThirdParty");
        var uri = new Uri(configuration[$"Services:{serviceName}:Api"]!);

        services
            .AddScoped<IServiceClientExecutor<TInterface>, ServiceClientExecutor<TInterface>>()
            .AddRefitClient<TInterface>(new RefitSettings
            {
                ExceptionFactory = async response =>
                {
                    if (response.IsSuccessStatusCode)
                        return null;

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                        return new ClientBadRequestException(problemDetails!);
                    }

                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return new ClientNotFoundException();

                    if (response.StatusCode == HttpStatusCode.NotAcceptable)
                        return new ClientNotAcceptableException();

                    var content = await response.Content.ReadAsStringAsync();
                    return new ClientException(response.StatusCode, content);
                },
                HttpRequestMessageOptions = new Dictionary<string, object>
                {
                    { "IsThirdParty", isThirdParty },
                    { "ServiceName", serviceName }
                }
            })
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = uri;
                client.Timeout = Timeout.InfiniteTimeSpan;
            })
            .AddHttpMessageHandler<Common.Handlers.HttpClientHandler>()
            .AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError().WaitAndRetryAsync(1, _ => TimeSpan.FromSeconds(5)));
    }

    public static void AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<Common.Handlers.HttpClientHandler>();

        services.RegisterService<IAuditLogServiceClient>(configuration);
        services.RegisterService<IEmoteServiceClient>(configuration);
        services.RegisterService<IImageProcessingClient>(configuration);
        services.RegisterService<IPointsServiceClient>(configuration);
        services.RegisterService<IRubbergodServiceClient>(configuration);
        services.RegisterService<IUserMeasuresServiceClient>(configuration);
        services.RegisterService<IRemindServiceClient>(configuration);
        services.RegisterService<ISearchingServiceClient>(configuration);
        services.RegisterService<IGraphicsClient>(configuration);
    }
}
