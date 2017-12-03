using System;
using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class PaletteChunk : ChunkItem
    {
        public PaletteChunk(ShockwaveReader input, ChunkEntry entry)
            : base(entry.Header)
        {
            var paletteCount = Math.Round((decimal)entry.Header.Length / 6);
        }
    }
}
