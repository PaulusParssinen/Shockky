using Shockky.Collections;
using Shockky.Resources.Cast;

namespace Shockky.Resources;

/// <summary>
/// Represents list of all cast members in the movie, sorted by the order they appear in the Score.
/// </summary>
[ShockwaveItem(BigEndian = true, IgnoreContainerEndianness = true, GenerateSerialization = true)]
public sealed partial class ScoreOrderResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.Sord;

    public ShockwaveList<CastMemberId> Entries { get; set; } = [];
}