using Shockky.IO;

namespace Shockky.Chunks
{
    public class FontMapChunk : BinaryDataChunk
    {
        public FontMapChunk()
            : base(ChunkKind.FXmp)
        { }
        public FontMapChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(ref input, header)
        { }
    }
}
