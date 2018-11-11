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
        public Point RegistrationPoint { get; set; }

        public BitmapCastFlags Flags { get; set; }
        public byte BitDepth { get; set; } = 1;
        public int Palette { get; set; }

        public BitmapCastProperties(CastMemberPropertiesChunk chunk, ShockwaveReader input)
        {
            bool IsDataAvailable()
                => (chunk.Header.Offset + chunk.Header.Length > input.Position);

            TotalWidth = input.ReadBigEndian<short>() & 0x7FFF;

            Rectangle = input.ReadRect();
            AlphaThreshold = input.ReadByte();

            input.Position += 7; //OLE

            short regX = input.ReadBigEndian<short>();
            short regY = input.ReadBigEndian<short>();
            RegistrationPoint = new Point(regX, regY);
            
            Flags = (BitmapCastFlags)input.ReadByte();
            
            if (!IsDataAvailable()) return;
            BitDepth = input.ReadByte();

            if (!IsDataAvailable()) return;
            Palette = input.ReadBigEndian<int>();
        }
        
        public int GetBodySize()
        {
            int size = 0;
            size += 2;
            size += sizeof(short) * 4;
            size += sizeof(byte);
            size += 7;
            size += sizeof(short);
            size += sizeof(short);
            size += sizeof(byte);

            if (BitDepth != 0)
                size += sizeof(byte);
            if (Palette != 0)
                size += sizeof(int);
            return size;
        }

        public void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException(nameof(BitmapCastProperties));
            output.Write(Rectangle);
            output.Write(AlphaThreshold);

            output.WriteBigEndian((short)RegistrationPoint.X);
            output.WriteBigEndian((short)RegistrationPoint.Y);

            output.Write((byte)Flags);
        }
    }
}
