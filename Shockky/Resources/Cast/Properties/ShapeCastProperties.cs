using System.Drawing;

using Shockky.IO;

namespace Shockky.Resources.Cast;

public class ShapeCastProperties : IMemberProperties
{
    public ShapeType Type { get; set; }
    public Rectangle Rectangle { get; set; }
    public short Pattern { get; set; }
    public byte ForegroundColor { get; set; }
    public byte BackgroundColor { get; set; }
    public bool IsFilled { get; set; }
    public InkType Ink { get; set; }
    public byte LineSize { get; set; }
    public byte LineDirection { get; set; }

    public ShapeCastProperties()
    { }
    public ShapeCastProperties(ref ShockwaveReader input, ReaderContext context)
    {
        Type = (ShapeType)input.ReadInt16BigEndian();
        Rectangle = input.ReadRectBigEndian();

        Pattern = input.ReadInt16BigEndian();
        ForegroundColor = input.ReadByte();
        BackgroundColor = input.ReadByte();

        byte flags = input.ReadByte(); //TODO:
        IsFilled = (flags << 1) == 1;
        Ink = (InkType)(flags & 0x3F);

        // csnover:
        // Director does not normalise file data, nor data to/from Lingo,
        // so this value can be anything 0-255. Only in the paint function
        // does it get clamped by (effectively) `max(0, (line_size & 0xf) - 1)`.
        LineSize = (byte)(input.ReadByte() - 1);
        LineDirection = (byte)(input.ReadByte() - 5);
    }

    public int GetBodySize(WriterOptions options)
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

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt16BigEndian((short)Type);
        output.WriteRect(Rectangle);

        output.WriteInt16BigEndian(Pattern);
        output.WriteByte(ForegroundColor);
        output.WriteByte(BackgroundColor);

        output.WriteByte((byte)(IsFilled ? 1 : 0)); //TODO:
        output.WriteByte((byte)(LineSize + 1));
        output.WriteByte((byte)(LineDirection + 5));
    }
}