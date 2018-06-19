using System;
using System.Collections.Generic;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class CastAssociationTableChunk : ChunkItem
    {
        public List<int> Members { get; }

        public CastAssociationTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Members = new List<int>((int)header.Length / 4);

            for (int i = 0; i < Members.Capacity; i++)
            {
                int castSlot = input.ReadBigEndian<int>();

               // if (castSlot == 0) continue; //TODO: check if castSlot == 0 matters

                Members.Add(castSlot);

                Console.WriteLine(castSlot);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            for(int i = 0; i < Members.Count; i++)
            {
                output.WriteBigEndian(Members[i]);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += Members.Count * sizeof(int);
            return size;
        }
    }
}
