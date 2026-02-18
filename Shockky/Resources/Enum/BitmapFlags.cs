namespace Shockky.Resources;

[Flags]
public enum BitmapFlags : byte
{
    None,
    Dither = 1 << 0,
    CenterRegistrationPoint = 1 << 5,
    Flag64 = 1 << 6,
    TrimWhitespace = 1 << 7,
}