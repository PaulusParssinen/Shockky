namespace Shockky.Resources;

[Flags]
public enum ChunkEntryFlags : ushort
{
    None = 0,
    Dirty = 1 << 0,
    Invalid = 1 << 2,
    Free = 1 << 3,
    UNK_10 = 1 << 4,
    PreloadPending = 1 << 5,
    Preloaded = 1 << 6,
    Allocated = 1 << 7,
    UNK_8000 = 1 << 15
}