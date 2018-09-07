using System;
using System.Linq;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class NameTableChunk : ChunkItem
    {
        public List<string> Names { get; set; }

        public int Length { get; set; }
        public int Length2 { get; set; }

        public NameTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Length = input.ReadBigEndian<int>();
            Length2 = input.ReadBigEndian<int>();

            short nameOffset = input.ReadBigEndian<short>();
            
            Names = new List<string>(input.ReadBigEndian<short>());

            input.Position = Header.Offset + nameOffset;
            for (int i = 0; i < Names.Capacity; i++)
            {
                Names.Add(input.ReadString());
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write(Length);
            output.Write(Length2);
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
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
