using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Chunks
{
    [DebuggerDisplay("[{Header.Kind}] Id: {Id} Offset: {Offset}")]
    public class AfterBurnerMapEntry : ShockwaveItem
    {
        public ChunkHeader Header { get; }

        public int Id { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public int DecompressedLength { get; set; }
        public int CompressionType { get; set; }

        public bool IsCompressed => (CompressionType == 0);

        public AfterBurnerMapEntry(DeflateShockwaveReader input)
        {
            Id = input.ReadVarInt();
            Offset = input.ReadVarInt();
            Length = input.ReadVarInt();
            DecompressedLength = input.ReadVarInt();
            CompressionType = input.ReadVarInt();

            Header = new ChunkHeader((ChunkKind)input.ReadBEInt32())
            {
                Length = IsCompressed ? DecompressedLength : Length
            };
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += ShockwaveWriter.GetVarIntSize(Id);
            size += ShockwaveWriter.GetVarIntSize(Offset);
            size += ShockwaveWriter.GetVarIntSize(Length);
            size += ShockwaveWriter.GetVarIntSize(DecompressedLength);
            size += ShockwaveWriter.GetVarIntSize(CompressionType);
            size += sizeof(int);
            return size;
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.WriteVarInt(Id);
            output.WriteVarInt(Offset);
            output.WriteVarInt(Length);
            output.WriteVarInt(DecompressedLength);
            output.WriteVarInt(CompressionType);
            output.WriteBE((int)Header.Kind);
        }
    }
}
