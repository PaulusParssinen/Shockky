using System;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class PaletteChunk : ChunkItem
    {
        public PaletteChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            //while(input.IsDataAvailable) test dis | TODO:

            var paletteCount = Math.Round((decimal)header.Length / 6);
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }

        public override int GetBodySize()
        {
            throw new NotImplementedException();
        }
    }
}
