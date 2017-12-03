using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks
{
    public class MemoryMapChunk : ChunkItem
    {
        public List<ChunkEntry> ChunkEntries { get; set; }

        public int ChunkCountMax { get; set; }
        public int ChunksUsed { get; set; } //idk whatsup with this

        public MemoryMapChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            input.ReadInt16(); //UNK0
            input.ReadInt16(); //UNK1

            ChunkCountMax = input.ReadInt32();
            ChunksUsed = input.ReadInt32();

            input.ReadInt32(); //junkPointer
            input.ReadInt32(); //UNK2
            input.ReadInt32(); //freePointer

            ChunkEntries = new List<ChunkEntry>(ChunksUsed);

            for (int i = 0; i < ChunksUsed; i++)
            {
                var chunkEntry = new ChunkEntry(ref input);
                ChunkEntries.Add(chunkEntry);
            }
        }
    }
}
