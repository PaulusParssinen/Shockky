namespace Shockky.Resources;

[Flags]
public enum FileInfoFlags : uint
{
    None,
    /// <summary>
    /// Pause playback when the movie window is not focused.
    /// </summary>
    PauseWhenUnfocused = 1 << 0,
    /// <summary>
    /// Disable anti-aliasing of text and graphics.
    /// </summary>
    NoAntiAliasing = 1 << 5,
    /// <summary>
    /// Remap the palettes of cast members to the closest colours in the active palette.
    /// </summary>
    RemapPalettes = 1 << 6,
    /// <summary>
    /// The movie was upgraded from D3 to D4
    /// </summary>
    UpgradedFromD3ToD4 = 1 << 7,
    /// <summary>
    /// Allows obsolete Lingo to be used in upgraded movies.
    /// </summary>
    AllowOutdatedLingo = 1 << 8,
    UpdateMovieEnabled = 1 << 9,
    PreloadEventAbort = 1 << 10,
    UnkD4 = 1 << 11,
    OutdatedSharedCast = 1 << 12,
    MovieField4D = 1 << 13,
}