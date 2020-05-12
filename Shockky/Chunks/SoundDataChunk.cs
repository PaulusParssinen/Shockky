using Shockky.IO;

namespace Shockky.Chunks
{
    public class SoundDataChunk : BinaryDataChunk
    {
        public SoundDataChunk()
            : base(ChunkKind.snd)
        { }
        public SoundDataChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(ref input, header)
        { }
    }
}
