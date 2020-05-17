using System.Drawing;

using Shockky.IO;

namespace Shockky.Chunks
{
    public class FavoriteColorsChunk : ChunkItem
    {
        public Color[] Colors { get; } = new Color[16];

        //TODO: Defaults, thanks Anthony!
        //Color.FromArgb(0, 0, 0),
        //Color.FromArgb(17, 17, 17), 
        //Color.FromArgb(34, 34, 34),
        //Color.FromArgb(51, 51, 51),
        //Color.FromArgb(68, 68, 68),
        //Color.FromArgb(85, 85, 85),
        //Color.FromArgb(102, 102, 102)
        //Color.FromArgb(119, 119, 119)
        //Color.FromArgb(136, 136, 136)
        //Color.FromArgb(153, 153, 153)
        //Color.FromArgb(170, 170, 170)
        //Color.FromArgb(187, 187, 187)
        //Color.FromArgb(204, 204, 204)
        //Color.FromArgb(221, 221, 221)
        //Color.FromArgb(238, 238, 238)
        //Color.FromArgb(255, 255, 255)

        public FavoriteColorsChunk()
            : base(ChunkKind.FCOL)
        { }
        public FavoriteColorsChunk(ref ShockwaveReader input, ChunkHeader header)
            : base(header)
        {
            Remnants.Enqueue(input.ReadInt32()); //1
            Remnants.Enqueue(input.ReadInt32()); //1
            
            for (int i = 0; i < Colors.Length; i++)
            {
                Colors[i] = Color.FromArgb(input.ReadByte(), input.ReadByte(), input.ReadByte());
            }
        }

        public override int GetBodySize()
        {
            int size = 0;
            size += sizeof(int);
            size += sizeof(int);

            size += Colors.Length * 3;
            return size;
        }

        public override void WriteBodyTo(ShockwaveWriter output)
        {
            output.Write((int)Remnants.Dequeue());
            output.Write((int)Remnants.Dequeue());

            foreach (Color color in Colors)
            {
                output.Write(color.R);
                output.Write(color.G);
                output.Write(color.B);
            }
        }
    }
}
