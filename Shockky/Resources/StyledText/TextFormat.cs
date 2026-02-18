using System.Drawing;

using Shockky.IO;

namespace Shockky.Resources;

public sealed class TextFormat : IShockwaveItem
{
    public int Offset { get; set; }
    public short Height { get; set; }
    public short Ascent { get; set; }
    public short FontId { get; set; }
    public bool Slant { get; set; }
    public byte Padding { get; set; }
    public short FontSize { get; set; }
    public Color Color { get; set; }

    public TextFormat(ref ShockwaveReader input, ReaderContext context)
    {
        Offset = input.ReadInt32LittleEndian();
        Height = input.ReadInt16LittleEndian();
        Ascent = input.ReadInt16LittleEndian();
        FontId = input.ReadInt16LittleEndian();
        Slant = input.ReadBoolean();
        Padding = input.ReadByte();
        FontSize = input.ReadInt16LittleEndian();
        Color = input.ReadColor();
    }

    public int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += sizeof(int);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(bool);
        size += sizeof(byte);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        size += sizeof(short);
        return size;
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        output.WriteInt32LittleEndian(Offset);
        output.WriteInt16LittleEndian(Height);
        output.WriteInt16LittleEndian(Ascent);
        output.WriteInt16LittleEndian(FontId);
        output.WriteBoolean(Slant);
        output.WriteByte(Padding);
        output.WriteInt16LittleEndian(FontSize);
        output.WriteColor(Color);
    }
}