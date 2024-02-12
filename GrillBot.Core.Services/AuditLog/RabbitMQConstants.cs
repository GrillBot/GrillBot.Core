namespace GrillBot.Core.Services.AuditLog;

public static class RabbitMQConstants
{
    public const string CreateItemQueue = "audit_log:create_item";
    public const string DeleteItemQueue = "audit_log:delete_item";
    public const string DeletedFileQueue = "audit_log:deleted_file";
}
