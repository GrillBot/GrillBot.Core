namespace GrillBot.Core.Services.AuditLog.Models.Events;

public class FileDeletePayload
{
    public string Filename { get; set; } = null!;

    public FileDeletePayload()
    {
    }

    public FileDeletePayload(string filename)
    {
        Filename = filename;
    }
}
