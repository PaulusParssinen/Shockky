using Shockky.IO;

namespace Shockky.Resources.Cast;

public sealed class ButtonCastProperties : TextCastProperties, IMemberProperties
{
    public ButtonType ButtonType { get; set; }

    public ButtonCastProperties()
    { }
    public ButtonCastProperties(ref ShockwaveReader input, ReaderContext context)
        : base(ref input, context)
    {
        ButtonType = (ButtonType)input.ReadInt16BigEndian();
    }

    public new int GetBodySize(WriterOptions options)
    {
        int size = 0;
        size += base.GetBodySize(options);
        size += sizeof(short);
        return size;
    }

    public new void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        base.WriteTo(output, options);
        output.WriteInt16BigEndian((short)ButtonType);
    }
}