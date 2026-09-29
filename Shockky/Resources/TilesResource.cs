using System.Drawing;

using Shockky.IO;
using Shockky.Resources.Cast;

namespace Shockky.Resources;

[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public sealed partial class Tile : IShockwaveItem<Tile>
{
    [PadBefore(4)]
    public CastMemberId Id { get; set; }
    public Rectangle Rect { get; set; }
}

[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public sealed partial class TilesResource : IResource, IShockwaveItem
{
    public OsType Kind => OsType.VWTL;

    [Count(8)]
    public Tile[] Items { get; set; } = new Tile[8];
}