using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class CastAssociationTableChunk : ChunkItem
    {
        public int[] Members { get; }

        public CastAssociationTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Members = new int[(int)header.Length / sizeof(int)];

            for (int i = 0; i < Members.Length; i++)
            {
                Members[i] = input.ReadBigEndian<int>();

                // if (castSlot == 0) continue;
            }
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
