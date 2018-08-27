using System.Collections.Generic;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class IndexMapChunk : ChunkItem
    {
        public List<int> MemoryMapOffsets { get; }

        public IndexMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            MemoryMapOffsets = new List<int>(input.ReadInt32());
            for (int i = 0; i < MemoryMapOffsets.Capacity; i++)
            {
                MemoryMapOffsets.Add(input.ReadInt32());
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int) * MemoryMapOffsets.Count;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(MemoryMapOffsets.Count);
            for (int i = 0; i < MemoryMapOffsets.Count; i++)
            {
                output.Write(MemoryMapOffsets[i]);
            }
        }
    }
}
