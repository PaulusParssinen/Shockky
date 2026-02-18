using Shockky.IO;

namespace Shockky.Resources;

public class SoundData : IResource, IBinaryData
{
    public OsType Kind => OsType.snd;

    public byte[] Data { get; set; }

    public SoundData(ref ShockwaveReader input)
    { }
}