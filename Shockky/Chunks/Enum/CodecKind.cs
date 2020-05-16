namespace Shockky.Chunks
{
    public enum CodecKind : int
    {
        Unknown = -1,

        MV85 = 0x4D563835,
        MV93 = 0x4D563933,
        MC93 = 0x4D433933,
        MC95 = 0x4D433935,

        //Afterburned
        FGDM = 0x4647444D,
        FGDC = 0x46474443
    }
}
