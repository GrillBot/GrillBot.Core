﻿using Discord;

namespace GrillBot.Core.Services.PointsService.Models;

public class MessageInfo
{
    public string Id { get; set; } = null!;
    public int ContentLength { get; set; }
    public MessageType MessageType { get; set; }
    public string AuthorId { get; set; } = null!;
}
