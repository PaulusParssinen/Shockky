using System;

namespace Shockky.Shockwave.Chunks.Enum
{
    [Flags]
    public enum BitmapCastFlags : byte
    {
        None,
        Dither                  = 1 << 0,
        CenterRegistrationPoint = 1 << 5,
        TrimWhitespace          = 1 << 7,
    }
}
