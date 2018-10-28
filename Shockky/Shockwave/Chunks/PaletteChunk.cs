using System.Drawing;
using System;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class PaletteChunk : ChunkItem
    {
        public Color[] Colors { get; }

        public PaletteChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Colors = new Color[header.Length / 6];
            for (int i = 0; i < Colors.Length; i++)
            {
                int r = input.ReadInt16();
                int g = input.ReadInt16();
                int b = input.ReadInt16();
                Colors[i] = Color.FromArgb(r, g, b);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new NotImplementedException();
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += (Colors.Length * 6);
            return size;
        }
    }
}
