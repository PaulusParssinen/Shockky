using Shockky.IO;
using Shockky.Shockwave.Chunks.Cast;
using System.Collections.Generic;
using System.Linq;

namespace Shockky.Shockwave.Chunks
{
    public class AssociationTableChunk : ChunkItem
    {
        public short Unknown1 { get; }
        public short Unknown2 { get; }

        public int Unknown3 { get; set; }

        public List<CastEntry> CastEntries { get; set; }

        public AssociationTableChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            Unknown1 = input.ReadInt16();
            Unknown2 = input.ReadInt16();
            Unknown3 = input.ReadInt32();

            CastEntries = new List<CastEntry>(input.ReadInt32());
            for (int i = 0; i < CastEntries.Capacity; i++)
            {
                CastEntries.Add(new CastEntry(input));
            }
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Unknown1);
            output.Write(Unknown2);
            output.Write(Unknown3);
            output.Write(CastEntries.Count); //TODO: nil check
            foreach (var entry in CastEntries)
            {
                entry.WriteTo(output);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(int);
            size += sizeof(int);
            size += CastEntries.Sum(ce => ce.GetBodySize());
            return size;
        }
    }
}
