using System;

namespace Shockky.Chunks
{
    [Flags]
    public enum ChunkEntryFlags
    {
        Load   = 1 << 0,
        Ignore = 1 << 2,
        Skip   = 1 << 3,
    }
}
