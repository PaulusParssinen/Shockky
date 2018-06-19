using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks.Cast
{
    public class CastEntry : ShockwaveItem
    {
        protected override string DebuggerDisplay => $"Slot: {Slot}";

        public int FileSlot { get; set; }
        public int Slot { get; set; }

        public string Name { get; set; }
        public ChunkKind Kind { get; set; }

        public CastEntry(ShockwaveReader input)
        {
            FileSlot = input.ReadInt32();
            Slot = input.ReadInt32();

	        Name = input.ReadReversedString(4);
            Kind = Name.ToChunkKind();
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
            output.Write(FileSlot);
            output.Write(Slot);
            output.WriteBigEndian(Name);
        }
    }
}
