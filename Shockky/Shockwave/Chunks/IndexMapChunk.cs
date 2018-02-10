using System.Collections.Generic;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class IndexMapChunk : ChunkItem
    {
        public List<int> MemoryMapOffsets;

        public IndexMapChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            MemoryMapOffsets = input.ReadList<int>(input.ReadInt32());
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int) * MemoryMapOffsets.Count;
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
            => throw new System.NotImplementedException();
    }
}
