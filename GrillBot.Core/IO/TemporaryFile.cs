﻿namespace GrillBot.Core.IO;

public sealed class TemporaryFile : IDisposable
{
    public string Path { get; private set; }

    public TemporaryFile(string extension)
    {
        Path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"{System.IO.Path.GetRandomFileName()}.{extension}");
    }

    public void ChangeExtension(string newExtension)
        => Path = System.IO.Path.ChangeExtension(Path, newExtension);

    public void Dispose()
    {
        if (File.Exists(Path))
            File.Delete(Path);
    }

    public override string ToString() => Path;
}
