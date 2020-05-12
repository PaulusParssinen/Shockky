using System;
using System.Drawing;

using Shockky.IO;

namespace Shockky.Chunks.Cast
{
    public class TextCastProperties : ICastProperties
    {
        public TextAlignment Alignment { get; set; }
        public Color BackgroundColor { get; set; }
        public short Font { get; set; }
        public Rectangle Rectangle { get; set; }
        public short LineHeight { get; set; }
        public short ButtonType { get; set; }

        public TextCastProperties()
        { }
        public TextCastProperties(ref ShockwaveReader input)
        {
            input.Advance(4);

            Alignment = (TextAlignment)input.ReadInt16();
            BackgroundColor = input.ReadColor();

            Font = input.ReadInt16();
            Rectangle = input.ReadRect();
            LineHeight = input.ReadInt16();

            input.Advance(4);
            ButtonType = input.ReadInt16();
        }

        public int GetBodySize()
        {
            int size = 0;
            size += 4;
            size += sizeof(short);
            size += 6;
            size += sizeof(short);
            size += sizeof(short) * 4;
            size += sizeof(short);
            size += 4;
            size += sizeof(short);
            return size;
        }

        public void WriteTo(ShockwaveWriter output)
        {
            throw new NotImplementedException(nameof(TextCastProperties));
            output.Write((short)Alignment);
            output.Write(BackgroundColor);

            output.Write(Font);
            output.Write(Rectangle);
            output.Write(LineHeight);


            output.Write(ButtonType);
        }
    }
}
