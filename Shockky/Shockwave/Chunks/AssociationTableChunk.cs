using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public class AssociationTableChunk : ChunkItem
    {
        public short EntrySize { get; }

        public int TotalCount { get; set; }
        public int AssignedCount { get; set; }

        public List<CastEntry> CastEntries { get; set; }

        public AssociationTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            EntrySize = input.ReadInt16(); //entry size, constant i think
            Remnants.Enqueue(input.ReadInt16());
            TotalCount = input.ReadInt32();
            AssignedCount = input.ReadInt32();
            CastEntries = new List<CastEntry>(TotalCount);
            for (int i = 0; i < CastEntries.Capacity; i++)
            {
                CastEntries.Add(new CastEntry(input));
            }

            //CastEntries.RemoveRange(AssignedCount, TotalCount - AssignedCount);
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(EntrySize);
            output.Write((short)Remnants.Dequeue());
            output.Write(TotalCount);
            output.Write(CastEntries.Count); //TODO: nil check
            foreach (var entry in CastEntries)
            {
                output.Write(entry);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);
            size += EntrySize * TotalCount;
            return size;
        }
    }
}
