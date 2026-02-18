using Shockky.IO;

namespace Shockky.Resources;

public sealed class CastMapResource : IResource, IShockwaveItem
{
    public OsType Kind => OsType.CASPtr;

    public int[] Members { get; set; }

    public CastMapResource()
    { }
    public CastMapResource(ref ShockwaveReader input, ReaderContext context)
    {
        input.ReverseEndianness = false;

        Members = new int[input.Length / sizeof(int)];
        for (int i = 0; i < Members.Length; i++)
        {
            Members[i] = input.ReadInt32BigEndian();
        }
    }

    public int GetBodySize(WriterOptions options) => Members.Length * sizeof(int);

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        for (int i = 0; i < Members.Length; i++)
        {
            output.WriteInt32LittleEndian(Members[i]);
        }
    }
}