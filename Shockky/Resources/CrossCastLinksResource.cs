using Shockky.IO;

namespace Shockky.Resources;

/// <summary>
/// The global cast library numbers and their paths are specified in this resource.
/// </summary>
public sealed class CrossCastLinksResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.ccl;

    public CrossCastLinksResource(ref ShockwaveReader input, ReaderContext context)
    {
        //TODO: VList<CrossCastLink>
    }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}

public record CrossCastLink(short GlobalCastLibNum, string Path);