using GrillBot.Core.Extensions;
using GrillBot.Core.Services.AuditLog;
using GrillBot.Core.Services.FileService;
using GrillBot.Core.Services.Graphics;
using GrillBot.Core.Services.ImageProcessing;
using GrillBot.Core.Services.PointsService;
using GrillBot.Core.Services.RubbergodService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrillBot.Core.Services;

public static class ServicesExtensions
{
    private static void AddHttpClient(this IServiceCollection services, IConfiguration configuration, string serviceId)
    {
        services.AddHttpClient(serviceId, client =>
        {
            client.BaseAddress = new Uri(configuration[$"Services:{serviceId}:Api"]!);
            client.Timeout = TimeSpan.FromMilliseconds(configuration[$"Services:{serviceId}:Timeout"]!.ToInt());
        });
    }

    private static void AddService<TInterface, TImplementation>(this IServiceCollection services, IConfiguration configuration, string serviceName)
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
        services.AddService<IFileServiceClient, FileServiceClient>(configuration, "FileService");
        services.AddService<IPointsServiceClient, PointsServiceClient>(configuration, "PointsService");
        services.AddService<IImageProcessingClient, ImageProcessingClient>(configuration, "ImageProcessing");
        services.AddService<IAuditLogServiceClient, AuditLogServiceClient>(configuration, "AuditLog");
    }
}
