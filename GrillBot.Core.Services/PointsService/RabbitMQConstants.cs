namespace GrillBot.Core.Services.PointsService;

public static class RabbitMQConstants
{
    public const string SynchronizationRequestQueue = "points:synchronization_requests";
    public const string CreateTransactionRequestQueue = "points:create_transaction_requests";
    public const string AdminCreateTransactionRequestQueue = "points:create_transaction_requests:admin";
    public const string DeleteTransactionQueue = "points:delete_transaction_requests";
}
