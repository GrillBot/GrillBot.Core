using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ;

public class RabbitMQConnectionFactory
{
    private IConfiguration Configuration { get; }

    public RabbitMQConnectionFactory(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConnection Create()
    {
        var configuration = Configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory
        {
            HostName = configuration["Hostname"],
            Password = configuration["Password"],
            UserName = configuration["Username"],
            DispatchConsumersAsync = true,
            RequestedHeartbeat = TimeSpan.FromSeconds(30)
        };

        return factory.CreateConnection();
    }
}
