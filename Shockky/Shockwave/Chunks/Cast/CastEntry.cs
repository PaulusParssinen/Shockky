using Shockky.IO;

namespace Shockky.Shockwave.Chunks.Cast
{
    public class CastEntry : ShockwaveItem
    {
        public int FileSlot { get; set; }
        public int Slot { get; set; }

        public string Name { get; set; }
        public ChunkKind Kind => Name.ToChunkKind();

        public CastEntry(ShockwaveReader input)
        {
            FileSlot = input.ReadInt32();
            Slot = input.ReadInt32();

	        Name = input.ReadReversedString(4);
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
            output.WriteReversedString(Name);
        }
    }
}
