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
            Colors = new Color[header.Length / 6];
            for (int i = 0; i < Colors.Length; i++)
            {
                //Oh god help me
                byte r = input.ReadBytes(2)[0];
                byte g = input.ReadBytes(2)[0];
                byte b = input.ReadBytes(2)[0];
                
                Colors[i] = Color.FromArgb(r, g, b);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            foreach (Color color in Colors)
            {
                output.Write(color.R);
                output.Write(color.R);

                output.Write(color.G);
                output.Write(color.G);

                output.Write(color.B);
                output.Write(color.B);
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
