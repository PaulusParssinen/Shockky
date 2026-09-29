namespace Shockky.Resources;

[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public sealed partial class LingoContextItem : IShockwaveItem<LingoContextItem>
{
    [PadBefore(4)] // Skip garbage ptr
    public int ResourceIndex { get; set; }

    public LingoContextItemFlags Flags { get; set; }

    /// <summary>
    /// Points to next free context item.
    /// </summary>
    public short Link { get; set; }
}