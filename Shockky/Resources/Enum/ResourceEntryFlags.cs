namespace Shockky.Resources;

[Flags]
public enum ResourceEntryFlags : ushort
{
    None = 0,
    Dirty = 1 << 0,
    /// <summary>
    /// This resource does not contain valid data.
    /// </summary>
    Invalid = 1 << 2,
    /// <summary>
    /// This resource is available for reuse.
    /// </summary>
    Free = 1 << 3,
    UNK_10 = 1 << 4,
    PreloadPending = 1 << 5,
    Preloaded = 1 << 6,
    Allocated = 1 << 7,
    UNK_8000 = 1 << 15
}