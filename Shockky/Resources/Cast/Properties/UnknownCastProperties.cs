using Shockky.IO;

namespace Shockky.Resources.Cast;

public class UnknownCastProperties : IMemberProperties
{
    public byte[] Data { get; set; }

    public UnknownCastProperties(ref ShockwaveReader input, int length)
    {
        Data = input.ReadBytes(length).ToArray();
    }

    public int GetBodySize(WriterOptions options) => Data.Length;
    public void WriteTo(ShockwaveWriter output, WriterOptions options) => output.WriteBytes(Data);
}