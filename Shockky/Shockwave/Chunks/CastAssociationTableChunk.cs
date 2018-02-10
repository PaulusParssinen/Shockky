using System;
using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class CastAssociationTableChunk : ChunkItem
    {
        public List<int> Members { get; }

        public CastAssociationTableChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            int amount = entry.Header.Length / 4;
            Members = new List<int>(amount + 1);
            
            for (int i = 0; i < amount; i++)
            {
                int castSlot = input.ReadBigEndian<int>();

                if(castSlot == 0) continue;

                Members[i + 1] = castSlot; //Add
            }
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }
    }
}
