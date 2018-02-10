using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class VWScoreChunk : ChunkItem
    {
        public VWScoreChunk(ShockwaveReader input, ChunkEntry entry) 
            : base(entry.Header)
        {

        }

        public override int GetBodySize()
        {
            throw new System.NotImplementedException();
        }

        public override void WriteTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();
        }
    }
}
