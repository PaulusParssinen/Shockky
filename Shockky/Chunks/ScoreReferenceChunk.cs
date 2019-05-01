using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class ScoreReferenceChunk : ChunkItem
    {
        public Dictionary<short, int> Entries { get; }

        public ScoreReferenceChunk(ShockwaveReader input, ChunkHeader header) 
            : base(header)
        {
            input.ReadBigEndian<int>();
            input.ReadBigEndian<int>();

            int entryCount = input.ReadBigEndian<int>();
            int validCount = input.ReadBigEndian<int>();

            input.ReadBigEndian<short>();
            input.ReadBigEndian<short>();

            Remnants.Enqueue(input.ReadBigEndian<int>());

            Entries = new Dictionary<short, int>(entryCount);
            for (int i = 0; i < entryCount; i++)
            {
                Entries.Add(input.ReadBigEndian<short>(), input.ReadBigEndian<int>());
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            size += Entries.Count * (sizeof(short) + sizeof(int));
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short ENTRY_OFFSET = 24;
            const short UNKNOWN = 8;
            
            output.WriteBigEndian(0);
            output.WriteBigEndian(0);

            output.WriteBigEndian(Entries.Count);
            output.WriteBigEndian(Entries.Count);

            output.WriteBigEndian(ENTRY_OFFSET);
            output.WriteBigEndian(UNKNOWN);

            output.WriteBigEndian((int)Remnants.Dequeue());
            foreach (var entry in Entries)
            {
                output.WriteBigEndian(entry.Key);
                output.WriteBigEndian(entry.Value);
            }
        }
    }
}
