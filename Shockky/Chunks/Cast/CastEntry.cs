using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    [DebuggerDisplay("[{Id}] {Kind}")]
    public class CastEntry : ShockwaveItem
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public ChunkKind Kind { get; set; }

        public CastEntry(ShockwaveReader input)
        {
            Id = input.ReadInt32();
            OwnerId = input.ReadInt32();
            Kind = input.ReadReversedString(4).ToChunkKind();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += 4;
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Id);
            output.Write(OwnerId);
            output.WriteReversedString(Kind.ToFourCC());
        }
    }
}
