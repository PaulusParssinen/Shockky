namespace Shockky.Collections;

/// <summary>
/// Represents the standard Director 5+ in-memory list header serialized to disk chunks (e.g. Sord, LctX, Lnam).
/// </summary>
[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public partial struct ListHeaderV5 : IListHeader, IShockwaveItem<ListHeaderV5>
{
    /// <summary>
    /// The number of active items in the list.
    /// The preceding 8 bytes contain the in-memory reference counting / vtable handle header.
    /// </summary>
    [PadBefore(8)]
    public int Used { get; set; }

    /// <summary>
    /// The allocated capacity of the list.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// The offset in bytes where items begin (header size).
    /// </summary>
    public short HeaderSize { get; set; } = 20;

    /// <summary>
    /// The size in bytes of each item.
    /// </summary>
    public short ItemSize { get; init; }

    public readonly int Count => Used;

    int IListHeader.Count
    {
        readonly get => Used;
        set
        {
            Used = value;
            Capacity = value;
        }
    }
}