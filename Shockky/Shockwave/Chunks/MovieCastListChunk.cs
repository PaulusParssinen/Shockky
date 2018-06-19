using System;
using System.Collections.Generic;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public class MovieCastListChunk : ChunkItem
    {
        public int CastCount { get; set; }

        public List<CastListEntry> Entries { get; set; }

        public MovieCastListChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            int unk1 = input.ReadBigEndian<int>();
            CastCount = input.ReadBigEndian<int>();
            int unk2 = input.ReadBigEndian<short>();
            int arraySize = input.ReadBigEndian<int>();

            var offsetTableApparently = input.ReadBytes(arraySize * 4); //5 integers?

            int castEntryLength = input.ReadBigEndian<int>();
            Entries = new List<CastListEntry>(CastCount);
            for(int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(new CastListEntry(input));
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += ((sizeof(int) * 4) * CastCount);
            size += sizeof(short);
            size += sizeof(int);
            //TODO
            return size;
        }
    }
}
