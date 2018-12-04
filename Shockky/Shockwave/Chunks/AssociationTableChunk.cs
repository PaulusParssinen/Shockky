using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public class AssociationTableChunk : ChunkItem
    {
        private const short ENTRY_SIZE = 12;

        public int TotalCount { get; set; }
        public int AssignedCount { get; set; }

        public List<CastEntry> CastEntries { get; set; }

        public AssociationTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadInt16();
            input.ReadInt16();
            int totalCount = input.ReadInt32();
            AssignedCount = input.ReadInt32();

            CastEntries = new List<CastEntry>(totalCount);
            for (int i = 0; i < CastEntries.Capacity; i++)
            {
                CastEntries.Add(new CastEntry(input));
            }

            //CastEntries.RemoveRange(AssignedCount, TotalCount - AssignedCount);
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write(ENTRY_SIZE);
            output.Write(ENTRY_SIZE);
            output.Write(CastEntries?.Count ?? 0);
            output.Write(CastEntries?.Count ?? 0);
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
            size += TotalCount * ENTRY_SIZE;
            return size;
        }
    }
}
