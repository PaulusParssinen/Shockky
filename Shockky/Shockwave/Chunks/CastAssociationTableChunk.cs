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
            var members = new int[amount + 1];
            
            for (int i = 0; i < amount; i++)
            {
                int castSlot = input.ReadInt32(true);

                if(castSlot == 0) continue;

                members[i + 1] = castSlot; //Add
            }

            Members = new List<int>(members);
        }
    }
}
