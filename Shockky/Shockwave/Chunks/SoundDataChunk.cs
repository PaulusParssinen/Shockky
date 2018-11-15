using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class SoundDataChunk : BinaryDataChunk
    {
        public SoundDataChunk(ShockwaveReader input, ChunkHeader header)
            : base(input, header)
        { }
    }
}
