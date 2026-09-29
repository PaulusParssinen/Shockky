using System.Drawing;

namespace Shockky.Resources.Cast;

// TODO: D3Mac does extra stuff on loading for versions < 1026: - src: csnover
[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public partial class TextCastProperties : IMemberProperties, IShockwaveItem<TextCastProperties>
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
}