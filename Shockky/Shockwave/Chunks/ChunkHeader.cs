using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("{Name}")]
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

        public int Id { get; set; }

        public string Name { get; set; }
        public ChunkKind Kind { get; set; }

        public long Offset { get; set; }
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
            Offset = input.Position;
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
            output.WriteReversedString(Name);
            output.Write(Length);
        }
    }
}
