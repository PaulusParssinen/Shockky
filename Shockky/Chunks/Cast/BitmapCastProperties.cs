using System.Drawing;

using Shockky.IO;
using Shockky.Chunks.Enum;

namespace Shockky.Chunks.Cast
{
    public class BitmapCastProperties : ICastTypeProperties
    {
        public int TotalWidth { get; set; }

        public Rectangle Rectangle { get; set; }
        public byte AlphaThreshold { get; set; }
        public byte[] OLE { get; }
        public Point RegistrationPoint { get; set; }

        public BitmapFlags Flags { get; set; }
        public byte BitDepth { get; set; } = 1;
        public int Palette { get; set; }

        public bool IsSystemPalette => ((Palette & (1 << 15)) != 0);

        public BitmapCastProperties()
        { }
        public BitmapCastProperties(ChunkHeader header, ShockwaveReader input)
        {
            bool IsDataAvailable()
                => input.Position < header.Offset + header.Length;

            TotalWidth = input.ReadBigEndian<ushort>() & 0x7FFF;

            Rectangle = input.ReadRect();
            AlphaThreshold = input.ReadByte();
            OLE = input.ReadBytes(7); //TODO:

            short regX = input.ReadBigEndian<short>();
            short regY = input.ReadBigEndian<short>();
            RegistrationPoint = new Point(regX, regY);
            
            Flags = (BitmapFlags)input.ReadByte();
            
            if (!IsDataAvailable()) return;
            BitDepth = input.ReadByte();

            if (!IsDataAvailable()) return;
            Palette = input.ReadBigEndian<int>();

            //TODO: PaletteRef or something
            if (!IsSystemPalette)
                Palette &= 0x7FFF;
        }
        
        public int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short) * 4;
            size += sizeof(byte);
            size += 7;
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(byte);

            if (BitDepth != 1)
                size += sizeof(byte);
            if (Palette != 0)
                size += sizeof(int);
            return size;
        }

        public void WriteTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((ushort)TotalWidth | 0x8000);

            output.Write(Rectangle);
            output.Write(AlphaThreshold);
            output.Write(OLE);

            output.WriteBigEndian((short)RegistrationPoint.X);
            output.WriteBigEndian((short)RegistrationPoint.Y);

            output.Write((byte)Flags);

            if (BitDepth == 1) return;
            output.Write(BitDepth);

            if (Palette != 0) return;
            if (!IsSystemPalette) Palette |= 0x8000;
            output.WriteBigEndian(Palette);
        }
    }
}
