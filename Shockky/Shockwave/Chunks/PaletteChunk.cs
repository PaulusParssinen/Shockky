using System;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class PaletteChunk : ChunkItem
    {
        public PaletteChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            for(int i = 0; i < (header.Length / 6); i++)
            {
                int r = input.ReadInt16();
                int g = input.ReadInt16();
                int b = input.ReadInt16();
            }
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
