namespace Shockky.Resources;

[Flags]
public enum TextFlags : byte
{
    None,
    Editable = 1 << 0,
    Tabbable = 1 << 1,
    NoWordWrap = 1 << 2
}