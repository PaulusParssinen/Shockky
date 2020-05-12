using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class SortOrderChunk : ChunkItem
    {
        public List<int> Entries { get; set; }

        public SortOrderChunk()
            : base(ChunkKind.Sord)
        { }
        public SortOrderChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());

            Entries = new List<int>(input.ReadInt32());

            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());

            for (int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(input.ReadInt32());
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
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            output.Write(Entries.Count);

            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            for (int i = 0; i < Entries.Count; i++)
            {
                output.Write(Entries[i]);
            }
        }
    }
}
