using Shockky.IO;
using System.Diagnostics;

namespace Shockky.Chunks
{
    [DebuggerDisplay("{Kind}")]
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

        public ChunkKind Kind { get; set; }
        
        public int Id { get; set; } //TODO: eh
        public int Offset { get; set; }
        public int Length { get; set; }

        public ChunkHeader(ChunkKind kind)
        {
            Kind = kind;
        }
        public ChunkHeader(ref ShockwaveReader input)
            : this((ChunkKind)input.ReadBEInt32())
        {
            Length = (IsVariableLength ? 
                input.Read7BitEncodedInt() : input.ReadBEInt32());
            Offset = input.Position; //TODO: How much we will rely on this? It's not even really true in some cases..
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
