using Shockky.Resources;

namespace Shockky.IO;

public readonly ref struct ReaderContext(DirectorVersion version)
{
    public readonly DirectorVersion Version { get; } = version;
}
