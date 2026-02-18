namespace Shockky.Resources;

[Flags]
public enum CastMemberFlags : short
{
    None = 0,
    DirtyMaybe = 1 << 0,

    DataModified = 1 << 2,
    PropertiesModified = 1 << 3,
    Locked = 1 << 4,

    FileNotFoundMaybe = 1 << 6,
    Unk80 = 1 << 7,
    NotPurgeable = 1 << 8,
    LinkedFile = 1 << 9,

    DataInMemory = 1 << 11,
}