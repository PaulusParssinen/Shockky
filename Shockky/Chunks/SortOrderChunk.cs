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
            Remnants.Enqueue(input.ReadInt32()); //TODO: I think I'm seeing a pattern here 🤔 See: ScriptContextChunk
            Remnants.Enqueue(input.ReadInt32()); //0 0, count, count, offset, entry length

            Entries = new List<int>(input.ReadInt32());
            input.ReadInt32();
            
            input.ReadInt16();
            input.ReadInt16();

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

            size += sizeof(short);
            size += sizeof(short);

            size += sizeof(int) * Entries.Count;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short ENTRIES_OFFSET = 20;
            const short ENTRY_SIZE = sizeof(int);

            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            output.Write(Entries.Count);
            output.Write(Entries.Count);

            output.Write(ENTRIES_OFFSET);
            output.Write(ENTRY_SIZE);

            for (int i = 0; i < Entries.Count; i++)
            {
                output.Write(Entries[i]);
            }
        }
    }
}
