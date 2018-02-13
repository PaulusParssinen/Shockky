using System.Collections.Generic;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class MemoryMapChunk : ChunkItem
    {
        public List<ChunkEntry> ChunkEntries { get; set; }

        public short Unknown1 { get; set; }
        public short Unknown2 { get; set; }
        public int Unknown3 { get; set; }

        public int JunkPtr { get; set; }
        public int FreePtr { get; set; }

        public int ChunkCountMax { get; } //TODO: Calculate these
        public int ChunksUsed { get; } //TODO: Calculate these

        public ChunkEntry this[int index]
            => ChunkEntries[index];

        public MemoryMapChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            Unknown1 = input.ReadInt16();
            Unknown2 = input.ReadInt16();

            ChunkCountMax = input.ReadInt32();
            ChunksUsed = input.ReadInt32();

            JunkPtr = input.ReadInt32();
            Unknown3 = input.ReadInt32();
            FreePtr = input.ReadInt32();

            ChunkEntries = new List<ChunkEntry>(ChunksUsed);

            for (int i = 0; i < ChunksUsed; i++)
            {
                ChunkEntries.Add(new ChunkEntry(input, i));
            }
        }

        //TODO: Adjust indexes method or somethign

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Unknown1);
            output.Write(Unknown2);
            output.Write(ChunkCountMax);
            output.Write(ChunksUsed);
            output.Write(JunkPtr);
            output.Write(Unknown3);
            output.Write(FreePtr);
            for (int i = 0; i < ChunkEntries.Count; i++)
            {
                output.Write(ChunkEntries[i]);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            return size;
        }
    }
}
