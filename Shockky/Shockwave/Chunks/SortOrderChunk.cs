using System;
using System.Collections.Generic;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class SortOrderChunk : ChunkItem
    {
        public List<int> Entries { get; set; }

        public SortOrderChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int unk0 = input.ReadBigEndian<int>();
            int unk1 = input.ReadBigEndian<int>();

            Entries = new List<int>(input.ReadBigEndian<int>());

            int unk2 = input.ReadBigEndian<int>();
            int unk3 = input.ReadBigEndian<int>();

            for (int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(input.ReadBigEndian<int>());
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int) * Entries.Count;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
