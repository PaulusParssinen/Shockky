using System;
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
            Id = input.Read7BitEncodedInt();
            Offset = input.Read7BitEncodedInt();
            Length = input.Read7BitEncodedInt();
            DecompressedLength = input.Read7BitEncodedInt();
            CompressionType = input.Read7BitEncodedInt();

            Debug.Assert(CompressionType == 1 || CompressionType == 0);

            Header = new ChunkHeader((ChunkKind)input.ReadBEInt32())
            {
                Length = IsCompressed ? DecompressedLength : Length
            };
        }

        public override int GetBodySize()
        {
            throw new NotSupportedException();
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            output.Write7BitEncodedInt(Id);
            output.Write7BitEncodedInt(Offset);
            output.Write7BitEncodedInt(Length);
            output.Write7BitEncodedInt(DecompressedLength);
            output.Write7BitEncodedInt(CompressionType);

            output.WriteBE((int)Header.Kind);
        }
    }
}
