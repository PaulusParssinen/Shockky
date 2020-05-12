using System;

namespace Shockky.Chunks
{
    [Flags]
    public enum VideoCastFlags : byte
    {
        None,

        Controller    = 1 << 0,
        Crop          = 1 << 1,
        Center        = 1 << 2,
        Video         = 1 << 3,
        DirectToStage = 1 << 4,
        InvertMask    = 1 << 5,
        Loop          = 1 << 6
    }
}
