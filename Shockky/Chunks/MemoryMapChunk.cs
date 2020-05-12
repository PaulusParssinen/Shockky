using Shockky.IO;

namespace Shockky.Chunks
{
    public class MemoryMapChunk : ChunkItem
    {
        public const short ENTRIES_OFFSET = 24;
        public const short ENTRY_SIZE = 20;

        public ChunkEntry[] Entries { get; set; }

        public int LastJunkId { get; set; }
        public int LastFreeId { get; set; }

        public ChunkEntry this[int index] => Entries[index];

        public MemoryMapChunk()
            : base(ChunkKind.mmap)
        { }
        public MemoryMapChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadBEInt16();
            input.ReadBEInt16();

            int entryCountMax = input.ReadBEInt32();
            int entryCount = input.ReadBEInt32();

            LastJunkId = input.ReadBEInt32();
            Remnants.Enqueue(input.ReadBEInt32());
            LastFreeId = input.ReadBEInt32();

            Entries = new ChunkEntry[entryCount];
            for (int i = 0; i < Entries.Length; i++)
            {
                var entry = new ChunkEntry(ref input);
                entry.Header.Id = i;

                Entries[i] = entry;
            }
        }
        
        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBE(ENTRIES_OFFSET);
            output.WriteBE(ENTRY_SIZE);

            output.WriteBE(Entries.Length); //TODO: I GUESS
            output.WriteBE(Entries.Length);

            output.WriteBE(LastJunkId);
            output.WriteBE((int)Remnants.Dequeue());
            output.WriteBE(LastFreeId);
            foreach (var entry in Entries)
            {
                entry.WriteTo(output);
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
