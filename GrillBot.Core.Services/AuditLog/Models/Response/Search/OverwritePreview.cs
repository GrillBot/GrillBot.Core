﻿using Discord;

namespace GrillBot.Core.Services.AuditLog.Models.Response.Search;

public class OverwritePreview
{
    public string TargetId { get; set; } = null!;
    public PermissionTarget TargetType { get; set; }
}
