using System;
using System.Collections.Generic;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public class AssociationTableChunk : ChunkItem
    {
        public List<CastEntry> CastEntries { get; set; }

        public AssociationTableChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            short unk1 = input.ReadInt16();
            short unk2 = input.ReadInt16();
            
            int unk3 = input.ReadInt32();

            int entryCount = input.ReadInt32();
            CastEntries = input.ReadList<CastEntry>(entryCount);
        }
    }
}
