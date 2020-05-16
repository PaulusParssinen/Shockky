using System;

using Shockky.IO;

namespace Shockky.Chunks
{
    //TODO: idk
    public class InitialLoadSegmentChunk : ChunkItem 
    {
        public InitialLoadSegmentChunk()
            : base(ChunkKind.ILS)
        { }
        public InitialLoadSegmentChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        { }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }
        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }
    }
}
