using System;
using System.Diagnostics;

using Shockky.IO;

namespace Shockky.Chunks
{
    [DebuggerDisplay("[{Header.Kind}] Id: {Id} Offset: {Offset} CompressedLength: {CompressedLength}")]
    public class AfterBurnerMapEntry : ShockwaveItem
    {
        public ChunkHeader Header { get; }

        public int Id { get; set; }
        public int Offset { get; set; }
        public int CompressedLength { get; set; }
        public int DecompressedLength { get; set; }
        public int CompressionType { get; set; } // TODO: Just 0 and 1?

        public bool IsCompressed => (CompressionType == 0); 

        public AfterBurnerMapEntry(DeflateShockwaveReader input)
        {
            Id = input.Read7BitEncodedInt();
            Offset = input.Read7BitEncodedInt();
            CompressedLength = input.Read7BitEncodedInt();
            DecompressedLength = input.Read7BitEncodedInt();
            CompressionType = input.Read7BitEncodedInt();

            Header = new ChunkHeader((ChunkKind)input.ReadInt32(true))
            {
                Length = DecompressedLength
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
            output.Write7BitEncodedInt(CompressedLength);
            output.Write7BitEncodedInt(DecompressedLength);
            output.Write7BitEncodedInt(CompressionType);

            output.WriteBE((int)Header.Kind);
        }
    }
}
