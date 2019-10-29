using System;

namespace Shockky.Lingo
{
    [Flags]
    public enum HandlerVectorFlags
    {
        MouseDown    = 1 << 0,
        MouseUp      = 1 << 1,
        KeyDown      = 1 << 2,
        KeyUp        = 1 << 3,

        PrepareFrame = 1 << 5,

        MouseEnter   = 1 << 7,
        MouseLeave   = 1 << 8,
        MouseWithin  = 1 << 9,

        StartMovie   = 1 << 11,
        StopMovie    = 1 << 12,

        ExitFrame    = 1 << 15,
    }
}
