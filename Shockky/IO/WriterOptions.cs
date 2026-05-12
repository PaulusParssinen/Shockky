using Shockky.Resources;

namespace Shockky.IO;

public readonly ref struct WriterOptions(DirectorVersion version)
{
    public DirectorVersion Version { get; } = version;
}