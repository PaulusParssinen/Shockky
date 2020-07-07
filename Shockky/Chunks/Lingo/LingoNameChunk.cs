using System.Linq;
using System.Collections.Generic;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class LingoNameChunk : ChunkItem
    {
        public List<string> Names { get; set; }

        public LingoNameChunk()
            : base(ChunkKind.Lnam)
        { }
        public LingoNameChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            input.IsBigEndian = true;

            Remnants.Enqueue(input.ReadInt32());
            Remnants.Enqueue(input.ReadInt32());
            input.ReadInt32();
            input.ReadInt32();

            short nameOffset = input.ReadInt16();
            
            Names = new List<string>(input.ReadInt16());

            input.Position = nameOffset;

            for (int i = 0; i < Names.Capacity; i++)
            {
                Names.Add(input.ReadString());
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            const short NAME_OFFSET = 20;
            int namesLength = Names?.Sum(n => sizeof(byte) + n.Length) ?? 0;

            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());
            output.Write(namesLength);
            output.Write(namesLength);
            output.Write(NAME_OFFSET);
            output.Write((short)Names.Count);

            foreach (string name in Names)
            {
                output.Write(name);
            }
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
