using System.Drawing;

namespace Shockky.Resources.Cast;

[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public partial class FilmLoopCastProperties : IMemberProperties, IShockwaveItem<FilmLoopCastProperties>
{
    public Rectangle Rectangle { get; set; }
    public FilmLoopFlags Flags { get; set; }
    public short Unk14 { get; set; }
}