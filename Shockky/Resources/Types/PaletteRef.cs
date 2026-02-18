using Shockky.Resources.Cast;

namespace Shockky.Resources.Types;

/// <summary>
/// Represents a reference to a palette cast member.
/// </summary>
public record struct PaletteRef(CastMemberId MemberId) //TODO: memNum 1-indexed
{
    public readonly bool IsSystemPalette => MemberId.MemberNum < 1;

    public readonly PaletteType Palette => (PaletteType)MemberId.MemberNum;
}

/// <summary>
/// Represents the built-in palettes available in Director
/// </summary>
public enum PaletteType : int
{
    Default = 0,
    /// <summary>
    /// "System - Mac"
    /// </summary>
    SystemMac = -1,
    Rainbow = -2,
    Grayscale = -3,
    Pastels = -4,
    Vivid = -5,
    NTSC = -6,
    Metallic = -7,
    Web216 = -8,
    VGA = -9,
    /// <summary>
    /// "System - Win (Dir 4)"
    /// </summary>
    SystemWinDir4 = -101,
    /// <summary>
    /// "System - Win"
    /// </summary>
    SystemWin = -102,
}