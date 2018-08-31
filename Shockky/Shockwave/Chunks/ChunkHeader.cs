using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Name} | {Length}")]
    public class ChunkHeader : ShockwaveItem
    {
        public bool IsVariableLength
        {
            get
            {
                switch (Kind)
                {
                    case ChunkKind.Fver:
                    case ChunkKind.Fcdr:
                    case ChunkKind.ABMP:
                    case ChunkKind.FGEI:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public string Name { get; set; }
        public ChunkKind Kind { get; set; }

        public long Length { get; set; }

        public ChunkHeader(ChunkKind kind)
        {
            Kind = kind;
        }
        public ChunkHeader(string name)
            : this(name.ToChunkKind())
        {
            Name = name;
        }
        public ChunkHeader(ShockwaveReader input)
            : this(input.ReadReversedString(4))
        {
            Length = (IsVariableLength ? 
                input.Read7BitEncodedInt() : input.ReadInt32());
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += 4;
            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write(Name); //fourcc so reverse TODO:
            output.Write(Length);
        }
    }
}
