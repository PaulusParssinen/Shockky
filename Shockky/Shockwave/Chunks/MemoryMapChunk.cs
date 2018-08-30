using System.Collections.Generic;
using System.Linq;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class MemoryMapChunk : ChunkItem
    {
        public List<ChunkEntry> Entries { get; set; }

        public int JunkPtr { get; set; }
        public int FreePtr { get; set; }

        public int ChunkCountMax { get; } //TODO: Calculate these
        public int ChunksUsed { get; } //TODO: Calculate these

        public ChunkEntry this[int index] 
            => Entries[index];
        
        public MemoryMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadInt16());
            Remnants.Enqueue(input.ReadInt16());

            ChunkCountMax = input.ReadInt32();
            ChunksUsed = input.ReadInt32();

            JunkPtr = input.ReadInt32();
            Remnants.Enqueue(input.ReadInt32());
            FreePtr = input.ReadInt32();

            Entries = new List<ChunkEntry>(ChunksUsed);

            for (int i = 0; i < ChunksUsed; i++)
            {
                Entries.Add(new ChunkEntry(input, i));
            }
        }

        //TODO: Adjust indexes method or somethign
        
        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((short)Remnants.Dequeue());
            output.Write((short)Remnants.Dequeue());
            output.Write(ChunkCountMax);
            output.Write(ChunksUsed);
            output.Write(JunkPtr);
            output.Write((int)Remnants.Dequeue());
            output.Write(FreePtr);
            for (int i = 0; i < Entries.Count; i++)
            {
                output.Write(Entries[i]);
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
            size += Entries.Sum(e => e.GetBodySize());
            return size;
        }
    }
}
