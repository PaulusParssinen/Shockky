using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class UnknownChunk : ChunkItem
    {
        public byte[] Data { get; set; }

        public UnknownChunk(ChunkHeader header, ShockwaveReader input)
            : base(header)
        {
            Data = input.ReadBytes(header.Length);
        }

        public override int GetBodySize()
        {
            return Data.Length;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Data);
        }
    }
}
