using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class BitmapChunk : BinaryDataChunk
    {
        public BitmapChunk(ShockwaveReader input, ChunkHeader header)
            : base(input, header)
        { }
    }
}
