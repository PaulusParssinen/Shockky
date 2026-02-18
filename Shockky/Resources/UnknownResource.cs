using Shockky.IO;

namespace Shockky.Resources;

public sealed class UnknownResource : IShockwaveItem, IResource
{
    public OsType Kind { get; }
    public byte[] Data { get; set; }

    public UnknownResource(ref ShockwaveReader input, ReaderContext context, OsType kind)
    {
        Kind = kind;

        Data = new byte[input.Length];
        input.ReadBytes(Data);
    }

    public int GetBodySize(WriterOptions options) => Data.Length;
    public void WriteTo(ShockwaveWriter output, WriterOptions options) => output.WriteBytes(Data);
}