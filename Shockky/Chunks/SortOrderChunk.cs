using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class SortOrderChunk : ChunkItem
    {
        public List<int> Entries { get; set; }

        public SortOrderChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            Entries = new List<int>(input.ReadBigEndian<int>());

            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());

            for (int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(input.ReadBigEndian<int>());
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int);

            size += sizeof(int);
            size += sizeof(int);

            size += sizeof(int) * Entries.Count;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());

            output.WriteBigEndian(Entries.Count);

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());

            for(int i = 0; i < Entries.Count; i++)
            {
                output.WriteBigEndian(Entries[i]);
            }
        }
    }
}
