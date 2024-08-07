﻿using GrillBot.Core.Services.AuditLog;
using GrillBot.Core.Services.Emote;
using GrillBot.Core.Services.Graphics;
using GrillBot.Core.Services.ImageProcessing;
using GrillBot.Core.Services.PointsService;
using GrillBot.Core.Services.RemindService;
using GrillBot.Core.Services.RubbergodService;
using GrillBot.Core.Services.SearchingService;
using GrillBot.Core.Services.UserMeasures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

    public static void AddExternalServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddService<IGraphicsClient, GraphicsClient>(configuration, "Graphics");
        services.AddService<IRubbergodServiceClient, RubbergodServiceClient>(configuration, "RubbergodService");
        services.AddService<IPointsServiceClient, PointsServiceClient>(configuration, "PointsService");
        services.AddService<IImageProcessingClient, ImageProcessingClient>(configuration, "ImageProcessing");
        services.AddService<IAuditLogServiceClient, AuditLogServiceClient>(configuration, "AuditLog");
        services.AddService<IUserMeasuresServiceClient, UserMeasuresServiceClient>(configuration, "UserMeasures");
        services.AddService<IEmoteServiceClient, EmoteServiceClient>(configuration, "Emote");
        services.AddService<IRemindServiceClient, RemindServiceClient>(configuration, "Remind");
        services.AddService<ISearchingServiceClient, SearchingServiceClient>(configuration, "Searching");
    }
}
