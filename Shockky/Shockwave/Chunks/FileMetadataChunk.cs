using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks
{
    public class FileMetadataChunk : ChunkItem
    {
        public long FileLength => Header.Length;

        public CodecKind Codec { get; set; }

        public FileMetadataChunk(ShockwaveReader input)
            : base(new ChunkHeader(input))
        {
            Codec = input.ReadReversedString(4).ToCodec();
        }
        public FileMetadataChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Codec = input.ReadReversedString(4).ToCodec();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += 4;
            return size;
        }
        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteBigEndian(Codec);
        }
    }
}
