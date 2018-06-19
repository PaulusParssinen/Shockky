using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class VWScoreChunk : ChunkItem
    {
        public VWScoreChunk(ShockwaveReader input, ChunkEntry entry) 
            : base(entry.Header)
        {
            int memoryHandleLength = input.ReadBigEndian<int>();
            int headerType = input.ReadBigEndian<int>(); //-3
            int spritePropertiesPositionsLengthPosition = input.ReadBigEndian<int>(); //12?
        }

        public override int GetBodySize()
        {
            throw new System.NotImplementedException();
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();
        }
    }
}
