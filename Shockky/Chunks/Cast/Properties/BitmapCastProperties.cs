using System.Drawing;

using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class BitmapCastProperties : ICastProperties
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
        public BitmapCastProperties(ChunkHeader header, ref ShockwaveReader input)
        {
            static bool IsDataAvailable(ShockwaveReader input, ChunkHeader header)
                => input.Position < header.Offset + header.Length;

            TotalWidth = input.ReadUInt16() & 0x7FFF;

            Rectangle = input.ReadRect();
            AlphaThreshold = input.ReadByte();
            OLE = input.ReadBytes(7).ToArray(); //TODO:

            RegistrationPoint = new Point(input.ReadInt16(), input.ReadInt16());
            
            Flags = (BitmapFlags)input.ReadByte();
            
            if (!IsDataAvailable(input, header)) return;
            BitDepth = input.ReadByte();

            if (!IsDataAvailable(input, header)) return;
            Palette = input.ReadInt32();

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
            output.Write((ushort)TotalWidth | 0x8000);

            output.Write(Rectangle);
            output.Write(AlphaThreshold);
            output.Write(OLE);

            output.Write((short)RegistrationPoint.X);
            output.Write((short)RegistrationPoint.Y);

            output.Write((byte)Flags);

            if (BitDepth == 1) return;
            output.Write(BitDepth);

            if (Palette != 0) return;
            if (!IsSystemPalette) Palette |= 0x8000;
            output.Write(Palette);
        }
    }
}
