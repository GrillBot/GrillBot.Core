using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ;

public class RabbitMQConnectionFactory
{
    private readonly IConfiguration _configuration;

    public RabbitMQConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IConnection Create()
    {
        var connectionFactory =
            CreateViaConnectionString() ??
            CreateViaParameters() ??
            throw new ArgumentException("Unable to find RabbitMQ connection string or parameters.");

        connectionFactory.DispatchConsumersAsync = true;
        connectionFactory.RequestedHeartbeat = TimeSpan.FromSeconds(30);

        return connectionFactory.CreateConnection();
    }

    private ConnectionFactory? CreateViaConnectionString()
    {
        var connectionString = _configuration.GetConnectionString("RabbitMQ");
        if (string.IsNullOrEmpty(connectionString))
            return null;

        return new ConnectionFactory
        {
            Uri = new Uri(connectionString)
        };
    }

    private ConnectionFactory? CreateViaParameters()
    {
        var configuration = _configuration.GetSection("RabbitMQ");
        if (!configuration.Exists())
            return null;

        return new ConnectionFactory
        {
            HostName = configuration["Hostname"],
            Password = configuration["Password"],
            UserName = configuration["Username"]
        };
    }
}
