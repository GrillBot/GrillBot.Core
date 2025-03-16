using GrillBot.Core.Services.AuditLog;
using GrillBot.Core.Services.Common.Exceptions;
using GrillBot.Core.Services.Common.Handlers;
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
using Refit;
using System.Net;
using System.Net.Http.Json;

namespace GrillBot.Core.Services;

public static class ServicesExtensions
{
    public static void AddHttpClient(this IServiceCollection services, IConfiguration configuration, string serviceId)
    {
        services.AddHttpClient(serviceId, client =>
        {
            client.BaseAddress = new Uri(configuration[$"Services:{serviceId}:Api"]!);
            client.Timeout = Timeout.InfiniteTimeSpan;
        });
    }

    public static void AddService<TInterface, TImplementation>(this IServiceCollection services, IConfiguration configuration, string serviceName)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        services
            .AddScoped<TInterface, TImplementation>()
            .AddHttpClient(configuration, serviceName);
    }

    public static void RegisterService<TInterface>(this IServiceCollection services, IConfiguration configuration, string serviceName)
        where TInterface : class
    {
        var isThirdParty = configuration.GetValue<bool>($"Services:{serviceName}:IsThirdParty");
        var uri = new Uri(configuration[$"Services:{serviceName}:Api"]!);

        services.AddScoped<AuthHeaderHandler>();

        services
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
                    { "IsThirdParty", isThirdParty }
                }
            })
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = uri;
                client.Timeout = Timeout.InfiniteTimeSpan;
            })
            .AddHttpMessageHandler<AuthHeaderHandler>();
    }

    public static void AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterService<IAuditLogServiceClient>(configuration, "AuditLog");
        services.RegisterService<IEmoteServiceClient>(configuration, "Emote");

        services.AddService<IGraphicsClient, GraphicsClient>(configuration, "Graphics");
        services.AddService<IRubbergodServiceClient, RubbergodServiceClient>(configuration, "RubbergodService");
        services.AddService<IPointsServiceClient, PointsServiceClient>(configuration, "PointsService");
        services.AddService<IImageProcessingClient, ImageProcessingClient>(configuration, "ImageProcessing");
        services.AddService<IUserMeasuresServiceClient, UserMeasuresServiceClient>(configuration, "UserMeasures");
        services.AddService<IRemindServiceClient, RemindServiceClient>(configuration, "Remind");
        services.AddService<ISearchingServiceClient, SearchingServiceClient>(configuration, "Searching");
    }
}
