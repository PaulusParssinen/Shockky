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
        public MovieCastListChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.IsBigEndian = true;

            Remnants.Enqueue(input.ReadInt32());
            Entries = new List<CastListEntry>(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt16());

            Unknowns = new int[input.ReadInt32()];
            for (int i = 0; i < Unknowns.Length; i++)
            {
                Unknowns[i] = input.ReadInt32();
            }

            input.ReadInt32();
            for (int i = 0; i < Entries.Capacity; i++)
            {
                Entries.Add(new CastListEntry(ref input));
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((int)Remnants.Dequeue());
            output.Write(Entries.Count);
            output.Write((int)Remnants.Dequeue());

            output.Write(Unknowns.Length);
            for (int i = 0; i < Remnants.Count; i++)
            {
                output.Write(Unknowns[i]);
            }

            output.Write(ENTRY_SIZE);
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
