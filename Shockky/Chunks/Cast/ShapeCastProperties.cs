using System.Drawing;

using Shockky.IO;
using Shockky.Chunks.Enum;

namespace Shockky.Chunks.Cast
{
    public class ShapeCastProperties : ICastTypeProperties
    {
        public ShapeType Type { get; set; }
        public Rectangle Rectangle { get; set; }
        public short Pattern { get; set; }
        public byte ForegroundColor { get; set; }
        public byte BackgroundColor { get; set; }
        public bool IsFilled { get; set; }
        public int LineSize { get; set; }
        public int LineDirection { get; set; }

        public ShapeCastProperties(ShockwaveReader input)
        {
            Type = (ShapeType)input.ReadBigEndian<short>();
            Rectangle = input.ReadRect();

            Pattern = input.ReadBigEndian<short>();
            ForegroundColor = input.ReadByte();
            BackgroundColor = input.ReadByte();
            IsFilled = input.ReadBoolean(); //TODO:
            LineSize = input.ReadByte(); //-1
            LineDirection = input.ReadByte(); //-5
        }

        public int GetBodySize()
        {
            int size = 0;
            size += sizeof(short);
            size += sizeof(short) * 4;
            size += sizeof(short);
            size += sizeof(byte);
            size += sizeof(byte);
            size += sizeof(bool);
            size += sizeof(byte);
            size += sizeof(byte);
            return size;
        }

        public void WriteTo(ShockwaveWriter output)
        {
            output.WriteBigEndian((short)Type);
            output.Write(Rectangle);

            output.WriteBigEndian(Pattern);
            output.Write(ForegroundColor);
            output.Write(BackgroundColor);

            output.Write(IsFilled);
            output.Write(LineSize);
            output.Write(LineDirection);
        }
    }
}
