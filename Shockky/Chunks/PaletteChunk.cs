using System.Drawing;

using Shockky.IO;

namespace Shockky.Chunks
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
                Colors[i] = input.ReadColor();
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            foreach (Color color in Colors)
            {
                output.Write(color);
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
