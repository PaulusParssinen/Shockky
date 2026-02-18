using System.Drawing;

using Shockky.IO;

namespace Shockky.Resources.Cast;

// TODO: D3Mac does extra stuff on loading for versions < 1026: - src: csnover
public class TextCastProperties : IMemberProperties
{
    public SizeType BorderSize { get; set; }
    public SizeType GutterSize { get; set; }
    public SizeType BoxShadowSize { get; set; }
    public TextBoxType BoxType { get; set; }

    public TextAlignment Alignment { get; set; }
    public Color BackgroundColor { get; set; }
    public short Font { get; set; }
    public Rectangle Rectangle { get; set; }
    public short LineHeight { get; set; }

    public SizeType TextShadowSize { get; set; }
    public TextFlags Flags { get; set; }

    public TextCastProperties()
    { }
    public TextCastProperties(ref ShockwaveReader input, ReaderContext context)
    {
        BorderSize = (SizeType)input.ReadByte();
        GutterSize = (SizeType)input.ReadByte();
        BoxShadowSize = (SizeType)input.ReadByte();
        BoxType = (TextBoxType)input.ReadByte();

        Alignment = (TextAlignment)input.ReadInt16BigEndian();
        BackgroundColor = input.ReadColor();
        Font = input.ReadInt16BigEndian();
        Rectangle = input.ReadRectBigEndian();
        LineHeight = input.ReadInt16BigEndian();

        TextShadowSize = (SizeType)input.ReadByte();
        Flags = (TextFlags)input.ReadByte();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(short);
        size += 6;
        size += sizeof(short);
        size += sizeof(short) * 4;
        size += sizeof(short);
        size += sizeof(byte);
        size += sizeof(byte);
        size += sizeof(short); //TODO:
        size += sizeof(short);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteByte((byte)BorderSize);
        output.WriteByte((byte)GutterSize);
        output.WriteByte((byte)BoxShadowSize);
        output.WriteByte((byte)BoxType);

        output.WriteInt16BigEndian((short)Alignment);
        output.WriteColor(BackgroundColor);

        output.WriteInt16BigEndian(Font);
        output.WriteRect(Rectangle);
        output.WriteInt16BigEndian(LineHeight);

        output.WriteByte((byte)TextShadowSize);
        output.WriteByte((byte)Flags);
    }
}