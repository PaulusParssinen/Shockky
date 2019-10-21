using Shockky.IO;

namespace Shockky.Chunks
{
    public class FontMapChunk : BinaryDataChunk
    {
        public FontMapChunk()
            : base(ChunkKind.FXmp)
        { }
        public FontMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(input, header)
        { }
    }
}
