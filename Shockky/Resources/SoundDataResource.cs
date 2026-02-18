using Shockky.IO;

namespace Shockky.Resources;

public class SoundDataResource : IResource, IBinaryDataResource
{
    public OsType Kind => OsType.snd;

    public byte[] Data { get; set; }

    public SoundDataResource(ref ShockwaveReader input)
    { }
}