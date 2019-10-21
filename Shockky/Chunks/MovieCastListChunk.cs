using System.Collections.Generic;

using Shockky.IO;
using Shockky.Chunks.Cast;

namespace Shockky.Chunks
{
    public class MovieCastListChunk : ChunkItem
    {
        private const int ENTRY_SIZE = 12;

        public int[] Unknowns { get; set; }
        public List<CastListEntry> Entries { get; set; }

        public MovieCastListChunk()
            : base(ChunkKind.MCsL)
        { }
        public MovieCastListChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Entries = new List<CastListEntry>(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<short>());

            Unknowns = new int[input.ReadBigEndian<int>()];
            for (int i = 0; i < Unknowns.Length; i++)
            {
                Unknowns[i] = input.ReadBigEndian<int>();
            }

            input.ReadBigEndian<int>();
            for (int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(new CastListEntry(input));
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian(Entries.Count);
            output.WriteBigEndian((int)Remnants.Dequeue());

            output.WriteBigEndian(Unknowns.Length);
            for (int i = 0; i < Remnants.Count; i++)
            {
                output.WriteBigEndian(Unknowns[i]);
            }

            output.WriteBigEndian(ENTRY_SIZE);
            for (int i = 0; i < Entries.Count; i++)
            {
                Entries[i].WriteTo(output);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(int);
            size += (Unknowns.Length * sizeof(int));
            size += sizeof(int);
            size += (Entries.Count * ENTRY_SIZE);
            return size;
        }
    }
}
