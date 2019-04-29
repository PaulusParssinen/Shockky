using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks.Cast;

namespace Shockky.Chunks
{
    public class AssociationTableChunk : ChunkItem
    {
        private const short ENTRY_SIZE = 12;
        
        public List<CastEntry> CastEntries { get; set; }
        public int AssignedCount { get; set; }

        public AssociationTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.ReadInt16();
            input.ReadInt16();
            CastEntries = new List<CastEntry>(input.ReadInt32());
            AssignedCount = input.ReadInt32();
            
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
            foreach (CastEntry entry in CastEntries)
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
            size += CastEntries.Count * ENTRY_SIZE;
            return size;
        }
    }
}
