using System.Buffers.Binary;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class FileMetadataChunk : ChunkItem
    {
        public CodecKind Codec { get; set; }

        public int FileLength => IsBigEndian ? BinaryPrimitives.ReverseEndianness(Header.Length) : Header.Length;
        public bool IsBigEndian => (Kind == ChunkKind.XFIR);

        public FileMetadataChunk()
            : base(ChunkKind.RIFX)
        { }
        public FileMetadataChunk(ref ShockwaveReader input)
            : this(ref input, new ChunkHeader(ref input))
        { }
        public FileMetadataChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Codec = (CodecKind)(IsBigEndian ? 
                input.ReadInt32() : input.ReadBEInt32());
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((int)Codec);
        }
    }
}