namespace Shockky.Resources;

public enum TransitionType
{
    None = 0,

    WipeRight,
    WipeLeft,
    WipeDown,
    WipeUp,

    CenterOutHorizontal,
    EdgesInHorizontal,

    CenterOutVertical,
    EdgesInVertical,

    CenterOutSquare,
    EdgesInSquare,

    PushLeft,
    PushRight,
    PushDown,
    PushUp,

    RevealUp,
    RevealUpRight,
    RevealRight,
    RevealDownRight,
    RevealDown,
    RevealDownLeft,
    RevealLeft,
    RevealUpLeft,

    DissolvePixelsFast,
    DissolveBoxyRects,
    DissolveBoxySquares,
    DissolvePatterns,

    RandomRows,
    RandomColumns,

    CoverDown,
    CoverDownLeft,
    CoverDownRight,
    CoverLeft,
    CoverRight,
    CoverUp,
    CoverUpLeft,
    CoverUpRight,

    VenetianBlind,
    Checkerboard,

    StripsBottomBuildLeft,
    StripsBottomBuildRight,
    StripsLeftBuildDown,
    StripsLeftBuildUp,
    StripsRightBuildDown,
    StripsRightBuildUp,
    StripsTopBuildLeft,
    StripsTopBuildRight,

    ZoomOpen,
    ZoomClose,

    VerticalBinds,

    DissolveBitsFast,
    DissolvePixels,
    DissolveBits
}