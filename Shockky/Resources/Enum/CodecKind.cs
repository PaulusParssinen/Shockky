namespace Shockky.Resources;

public enum CodecKind : uint
{
    Unknown = 0,

    MV85 = 0x4D563835,
    MV93 = 0x4D563933,
    MC93 = 0x4D433933,
    MC95 = 0x4D433935,

    /// <summary>
    /// Represents afterburned Director movie.
    /// </summary>
    FGDM = 0x4647444D,
    /// <summary>
    /// Represents afterburned Director cast.
    /// </summary>
    FGDC = 0x46474443
}