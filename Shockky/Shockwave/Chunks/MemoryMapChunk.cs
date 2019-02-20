using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class MemoryMapChunk : ChunkItem
    {
        public const short ENTRY_SIZE = 20;

        public ChunkEntry[] Entries { get; set; }

        public int LastJunkId { get; set; }
        public int LastFreeId { get; set; }

        public ChunkEntry this[int index] 
            => Entries[index];

        public MemoryMapChunk()
            : base(ChunkKind.mmap)
        { }
        public MemoryMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadInt16());
            input.ReadInt16();

            int entryCountMax = input.ReadInt32();
            int entryCount = input.ReadInt32();

            LastJunkId = input.ReadInt32();
            Remnants.Enqueue(input.ReadInt32());
            LastFreeId = input.ReadInt32();

            Entries = new ChunkEntry[entryCount];
            for (int i = 0; i < Entries.Length; i++)
            {
                var entry = new ChunkEntry(input);
                entry.Header.Id = i;

                Entries[i] = entry;
            }
        }
        
        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((short)Remnants.Dequeue());
            output.Write(ENTRY_SIZE);
            output.Write(Entries.Length); //TODO: I GUESS
            output.Write(Entries.Length);
            output.Write(LastJunkId);
            output.Write((int)Remnants.Dequeue());
            output.Write(LastFreeId);
            for (int i = 0; i < Entries.Length; i++)
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
            size += Entries.Length * ENTRY_SIZE;
            return size;
        }
    }
}
