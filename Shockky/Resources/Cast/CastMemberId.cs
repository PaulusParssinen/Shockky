namespace Shockky.Resources.Cast;

/// <summary>
/// Represents a reference to a cast member by library number and member number.
/// </summary>
[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public readonly partial record struct CastMemberId(short CastLib = 0, short MemberNum = 0) : IShockwaveItem<CastMemberId>;