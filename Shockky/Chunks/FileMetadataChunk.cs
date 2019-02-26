using Shockky.IO;

namespace Shockky.Chunks
{
    public class FileMetadataChunk : ChunkItem
    {
        private long _fileLength;
        public long FileLength
        {
            set { Header.Length = _fileLength = value; }
            get { return _fileLength = Header.Length; }
        }

        public CodecKind Codec { get; set; }

        public FileMetadataChunk()
            : base(ChunkKind.RIFX)
        { }
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

        public override int GetBodySize() => (int)FileLength;

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.WriteReversedString(Codec.ToString());
        }
    }
}