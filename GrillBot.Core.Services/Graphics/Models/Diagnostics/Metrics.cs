﻿namespace GrillBot.Core.Services.Graphics.Models.Diagnostics;

public class Metrics
{
    public MemoryInfo Mem { get; set; } = null!;
    public double Uptime { get; set; }
}
