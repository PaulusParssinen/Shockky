using System.Linq;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class NameTableChunk : ChunkItem
    {
        public List<string> Names { get; set; }

        public NameTableChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadBigEndian<int>());
            Remnants.Enqueue(input.ReadBigEndian<int>());
            input.ReadBigEndian<int>();
            input.ReadBigEndian<int>();

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
            const short NAME_OFFSET = 20;
            int namesLength = Names?.Sum(n => sizeof(byte) + n.Length) ?? 0;

            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian((int)Remnants.Dequeue());
            output.WriteBigEndian(namesLength);
            output.WriteBigEndian(namesLength);
            output.WriteBigEndian(NAME_OFFSET);
            output.WriteBigEndian((short)Names.Count);

            foreach (string name in Names)
                output.Write(name);
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
