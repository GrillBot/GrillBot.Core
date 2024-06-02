using GrillBot.Core.Validation;
using System.ComponentModel.DataAnnotations;

namespace GrillBot.Core.Services.RemindService.Models.Request;

public class CopyReminderRequest
{
    public long RemindId { get; set; }

    [DiscordId]
    [StringLength(32)]
    public string ToUserId { get; set; } = null!;

    [StringLength(32)]
    public string Language { get; set; } = null!;
}
