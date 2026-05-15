using System.Drawing;

namespace Shockky.Resources;

[ShockwaveItem(BigEndian = false, GenerateSerialization = true)]
public sealed partial class TextFormat : IShockwaveItem
{
    public int Offset { get; set; }
    public short Height { get; set; }
    public short Ascent { get; set; }
    public short FontId { get; set; }
    public bool Slant { get; set; }
    public byte Padding { get; set; }
    public short FontSize { get; set; }
    public Color Color { get; set; }
}