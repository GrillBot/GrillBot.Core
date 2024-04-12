using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrillBot.Core.RabbitMQ.Consumer;

public interface IRabbitHandler
{
    string QueueName { get; }
    Type PayloadType { get; }

    Task HandleEventAsync(object? payload, Dictionary<string, string> headers);
    Task HandleUnknownEventAsync(string message, Dictionary<string, string> headers);
}
