using System;
using System.Drawing;

using Shockky.IO;
using Shockky.Shockwave.Chunks.Enum;

namespace Shockky.Shockwave.Chunks.Cast
{
    public class ShapeCastProperties : ICastTypeProperties
    {
        public ShapeType Type { get; set; }
        public Rectangle Rectangle { get; set; }
        public short Pattern { get; set; }

        public bool IsFilled { get; set; }
        public int LineSize { get; set; }
        public int LineDirection { get; set; }

        public ShapeCastProperties(ShockwaveReader input)
        {
            Type = (ShapeType)input.ReadBigEndian<short>();
            Rectangle = input.ReadRect();

            Pattern = input.ReadBigEndian<short>();
            input.Position += 2;
            byte flags = input.ReadByte();
            IsFilled = (flags == 1); //TODO:
            LineSize = input.ReadByte(); //-1
            LineDirection = input.ReadByte(); // -5
        }

        public int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short) * 4;
            size += sizeof(short);
            size += 2;
            size += sizeof(byte);
            size += sizeof(byte);
            size += sizeof(byte);
            return size;
        }

        public void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException(nameof(ShapeCastProperties));
        }
    }
}
