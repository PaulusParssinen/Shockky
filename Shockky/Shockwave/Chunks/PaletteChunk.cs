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
                byte[] r = input.ReadBytes(2);
                byte[] g = input.ReadBytes(2);
                byte[] b = input.ReadBytes(2);

                /*
                int r = input.ReadBigEndian<ushort>() & 0x7F;
                int g = input.ReadBigEndian<ushort>() & 0x7F;
                int b = input.ReadBigEndian<ushort>() & 0x7F;
                */
                Colors[i] = Color.FromArgb(r[0], g[0], b[0]);
            }
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            throw new System.NotImplementedException();

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
