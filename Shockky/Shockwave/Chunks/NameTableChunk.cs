using System;
using System.Collections.Generic;
using System.Linq;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class NameTableChunk : ChunkItem
    {
        public List<string> Names { get; set; }

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }

        public int Length { get; set; }
        public int Length2 { get; set; }

        public NameTableChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            Unknown1 = input.ReadBigEndian<int>();
            Unknown2 = input.ReadBigEndian<int>();
            Length = input.ReadBigEndian<int>();
            Length2 = input.ReadBigEndian<int>();
            short nameOffset = input.ReadBigEndian<short>();
            short nameCount = input.ReadBigEndian<short>();

            Names = input.ReadList<string>(nameCount, nameOffset);
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Unknown1);
            output.Write(Unknown2);
            output.Write(Length);
            output.Write(Length2);
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(int);
            size += sizeof(short);
            size += sizeof(short);
            size += Names.Sum(n => n.Length + 1);
            return size;
        }
    }
}
