namespace Shockky.Resources;

/// <summary>
/// Character class identification and case folding map (CMAP chunk).
/// </summary>
[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public sealed partial class CharacterMapResource : IResource, IShockwaveItem
{
    public OsType Kind => OsType.CMAP;

    /// <summary>
    /// Fixed 256-element array of character attribute flags and case folding mappings.
    /// </summary>
    [Count(256)]
    public uint[] Map { get; set; } = new uint[256];
}