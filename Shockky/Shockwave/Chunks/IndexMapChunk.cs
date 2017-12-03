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
            MemoryMapOffsets = input
                .ReadIntList(input.ReadInt32(), 0, false); //Reads the _offsets_ of the memorymapchunks
        }
    }
}
