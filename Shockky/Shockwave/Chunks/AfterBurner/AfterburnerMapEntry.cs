using System;
using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Chunks.Interface;

namespace Shockky.Shockwave.Chunks
{
    [DebuggerDisplay("[{Header.Name}] Id: {Id}, Offset: {Offset}, CompressionType: {CompressionType}")]
    public class AfterBurnerMapEntry : ShockwaveItem, IChunkEntry
    {
        public ChunkHeader Header { get; set; }

        public int Id { get; set; }
        public int Offset { get; set; }
        public int CompressedLength { get; set; }
        public int DecompressedLength { get; set; }
        public EntryCompressionType CompressionType { get; set; }

        public bool IsCompressed => (CompressionType == EntryCompressionType.Compressed);

        public AfterBurnerMapEntry(ShockwaveReader input)
        {
            Id = input.Read7BitEncodedInt();
            Offset = input.Read7BitEncodedInt();
            CompressedLength = input.Read7BitEncodedInt();
            DecompressedLength = input.Read7BitEncodedInt();
            CompressionType = (EntryCompressionType)input.Read7BitEncodedInt();

            Header = new ChunkHeader(input.ReadReversedString(4))
            {
                Length = CompressedLength
            };
        }

        public override int GetBodySize()
        {
            throw new NotSupportedException();
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            //TODO TODO TODO
            throw new NotSupportedException();
        }
    }
}
