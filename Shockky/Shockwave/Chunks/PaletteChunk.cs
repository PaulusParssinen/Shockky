using System.Drawing;

using Shockky.IO;

namespace Shockky.Shockwave.Chunks
{
    public class PaletteChunk : ChunkItem
    {
        public Color[] Colors { get; set; }

        public PaletteChunk(ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Colors = new Color[(header.Length / 6)];
            for (int i = 0; i < Colors.Length; i++)
            {
                int r = input.ReadInt16() & 0x7F;
                int g = input.ReadInt16() & 0x7F;
                int b = input.ReadInt16() & 0x7F;

                Colors[i] = Color.FromArgb(r, g, b);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            foreach(var color in Colors)
            {
                output.Write((short)color.R);
                output.Write((short)color.G);
                output.Write((short)color.B);
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += (Colors.Length * 6);
            return size;
        }
    }
}
