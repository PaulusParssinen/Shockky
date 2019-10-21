using Shockky.IO;

namespace Shockky.Chunks
{
    public class SoundDataChunk : BinaryDataChunk
    {
        public SoundDataChunk()
            : base(ChunkKind.snd)
        { }
        public SoundDataChunk(ShockwaveReader input, ChunkHeader header)
            : base(input, header)
        { }
    }
}
