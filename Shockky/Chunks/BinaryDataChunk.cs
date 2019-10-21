using Shockky.IO;

namespace Shockky.Chunks
{
    public abstract class BinaryDataChunk : ChunkItem
    {
        public byte[] Data { get; set; }

        protected BinaryDataChunk(ChunkKind kind)
            : base(kind)
        { }
        protected BinaryDataChunk(ShockwaveReader input, ChunkHeader header) 
            : base(header)
        {
            Data = input.ReadBytes((int)header.Length);
        }

        public override int GetBodySize() => Data.Length;
        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(Data);
        }
    }
}
