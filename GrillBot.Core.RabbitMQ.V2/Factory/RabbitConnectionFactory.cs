using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace GrillBot.Core.RabbitMQ.V2.Factory;

public class RabbitConnectionFactory(IConfiguration _configuration) : IRabbitConnectionFactory
{
    public async Task<IConnection> CreateAsync()
    {
        var configuration = _configuration.GetSection("RabbitMQ");
        if (!configuration.Exists())
            throw new InvalidOperationException("Missing RabbitMQ configuration section.");

        var factory = new ConnectionFactory
        {
            HostName = configuration["Hostname"]!,
            Password = configuration["Password"]!,
            UserName = configuration["Username"]!,
            RequestedHeartbeat = TimeSpan.FromSeconds(30)
        };

        return await factory.CreateConnectionAsync();
    }
}
