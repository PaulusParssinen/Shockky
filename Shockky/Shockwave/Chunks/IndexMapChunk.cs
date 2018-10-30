using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class IndexMapChunk : ChunkItem
    {
        public int[] MemoryMapOffsets { get; }

        public IndexMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            MemoryMapOffsets = new int[input.ReadInt32()];
            for (int i = 0; i < MemoryMapOffsets.Length; i++)
            {
                MemoryMapOffsets[i] = input.ReadInt32();
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int) * MemoryMapOffsets.Length;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(MemoryMapOffsets.Length);
            for (int i = 0; i < MemoryMapOffsets.Length; i++)
            {
                output.Write(MemoryMapOffsets[i]);
            }
        }
    }
}
