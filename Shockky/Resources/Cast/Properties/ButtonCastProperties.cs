using System.Diagnostics.CodeAnalysis;

using Shockky.IO;

namespace Shockky.Resources.Cast;

public sealed class ButtonCastProperties : TextCastProperties, IMemberProperties, IShockwaveItem<ButtonCastProperties>
{
    public ButtonType ButtonType { get; set; }

    public ButtonCastProperties()
    { }

    [SetsRequiredMembers]
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

    public new void WriteTo(ref ShockwaveWriter output, WriterOptions options)
    {
        base.WriteTo(ref output, options);
        output.WriteInt16BigEndian((short)ButtonType);
    }

    public static new ButtonCastProperties Read(ref ShockwaveReader input, ReaderContext context) => new(ref input, context);
}