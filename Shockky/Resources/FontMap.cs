using Shockky.IO;

namespace Shockky.Resources;

public class FontMap : IShockwaveItem, IResource, IBinaryData
{
    public OsType Kind => OsType.Fmap;

    public byte[] Data { get; set; }

    public FontMap(ref ShockwaveReader input, ReaderContext context)
    {
        Data = new byte[input.Length];
        input.ReadBytes(Data);
    }

    public int GetBodySize(WriterOptions options) => Data.Length;
    public void WriteTo(ShockwaveWriter output, WriterOptions options) => output.WriteBytes(Data);
}