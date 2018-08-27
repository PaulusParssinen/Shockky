using System;
using System.Collections.Generic;

using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;

namespace Shockky.Shockwave.Chunks
{
    public class MovieCastListChunk : ChunkItem
    {
        public int EntryLength { get; }
        public List<CastListEntry> Entries { get; set; }

        public MovieCastListChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());

            Entries = new List<CastListEntry>(input.ReadBigEndian<int>());

            Remnants.Enqueue(input.ReadBigEndian<short>());
            int arraySize = input.ReadBigEndian<int>();

            var offsetTableApparently = input.ReadBytes(arraySize * 4); //integers?

            EntryLength = input.ReadBigEndian<int>();
            for(int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(new CastListEntry(input));
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian(Entries.Count);
            output.WriteBigEndian((int)Remnants.Dequeue());
            //TODO
            output.WriteBigEndian(EntryLength);
            for(int i = 0; i < Entries.Count; i++)
            {
                output.Write(Entries[i]);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(int);
            //TODO: Offset table
            size += sizeof(int);
            size += (Entries.Count * EntryLength);
            return size;
        }
    }
}
