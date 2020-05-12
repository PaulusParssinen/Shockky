using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class UnknownCastProperties : ShockwaveItem, ICastProperties
    {
        public byte[] Data { get; set; }

        public UnknownCastProperties(ref ShockwaveReader input, int length)
        {
            Data = input.ReadBytes(length).ToArray();
        }

        public override int GetBodySize() => Data.Length;
        public override void WriteTo(ShockwaveWriter output) => output.Write(Data);
    }
}
