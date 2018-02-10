using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Name} | {Length}")]
    public class ChunkHeader : ShockwaveItem
    {
        public int Index { get; set; }

        public string Name { get; set; }
        public ChunkType Type { get; set; }

        public int Length { get; set; }
        
        public ChunkHeader(ChunkType type)
        {
            Type = type;
        }
        public ChunkHeader(ShockwaveReader input, int index)
            : this(input)
        {
            Index = index;
        }
        public ChunkHeader(ShockwaveReader input)
        {
            Name = input.ReadReversedString(4);
            Type = Name.ToChunkType();
            Length = input.ReadInt32();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += 4; //fourCC
            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Name); //fourcc so reverse TODO
            output.Write(Length);
        }
    }
}
