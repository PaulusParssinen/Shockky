using System;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class FontMapChunk : ChunkItem
    {
        public FontMapChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            string fontmap = input.ReadString((int)header.Length);
        }

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
