using Shockky.IO;
using System.Collections.Generic;

namespace Shockky.Chunks
{
    public class CastAssociationTableChunk : ChunkItem
    {
        public int[] Members { get; }

        public CastAssociationTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            var members = new List<int>((int)header.Length / sizeof(int));
            
            for (int i = 0; i < members.Capacity; i++)
            {
                int id = input.ReadBigEndian<int>();

                if (id != 0)
                    members.Add(id);
            }

            Members = members.ToArray();
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            for(int i = 0; i < Members.Length; i++)
            {
                output.WriteBigEndian(Members[i]);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += Members.Length * sizeof(int);
            return size;
        }
    }
}
