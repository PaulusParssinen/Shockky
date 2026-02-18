namespace Shockky.Resources;

[Flags]
public enum FilmLoopFlags : int
{
    CropFromCenter = 1 << 0,
    Scale = 1 << 1,
    MapPalettes = 1 << 2,
    SoundEnabled = 1 << 3,
    EnableScripts = 1 << 4,
    NoLoop = 1 << 5
}