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

        public CastEntry() { }
        public CastEntry(ref ShockwaveReader input)
        {
            Id = input.ReadBEInt32();
            OwnerId = input.ReadBEInt32();
            Kind = (ChunkKind)input.ReadBEInt32();
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
            output.WriteBE(Id);
            output.WriteBE(OwnerId);
            output.Write((int)Kind);
        }
    }
}
