using Shockky.IO;

namespace Shockky.Chunks
{
    public class ChunkHeader : ShockwaveItem
    {
        protected override string DebuggerDisplay => Kind.ToString();

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

        public ChunkKind Kind { get; set; }
        public int Length { get; set; }

        public ChunkHeader(ChunkKind kind)
        {
            Kind = kind;
        }
        public ChunkHeader(ref ShockwaveReader input)
            : this((ChunkKind)input.ReadBEInt32())
        {
            Length = IsVariableLength ? 
                input.Read7BitEncodedInt() : input.ReadBEInt32();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int); //TODO: VariableLength => GetByteCount() or somethingg
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write((int)Kind);
            output.WriteBE(Length); //TODO: VariableLength
        }
    }
}
