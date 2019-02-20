using System;
using System.Drawing;

using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks.Cast
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

        public BitmapCastProperties(CastMemberPropertiesChunk chunk, ShockwaveReader input)
        {
            bool IsDataAvailable()
                => (chunk.Header.Offset + chunk.Header.Length > input.Position);

            TotalWidth = input.ReadBigEndian<ushort>() & 0x7FFF; //TODO: Research more

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
            Palette = input.ReadBigEndian<int>();// & 0x7FFF;
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
            output.WriteBigEndian(Palette);
        }
    }
}
