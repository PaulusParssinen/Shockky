namespace Shockky.Resources.Cast;

[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public sealed partial class CastListEntry : IShockwaveItem<CastListEntry>
{
    [ParseStringAs(StringParseKind.PString)]
    public string Name { get; set; } = string.Empty;

    [ParseStringAs(StringParseKind.PString)]
    public string FilePath { get; set; } = string.Empty;

    public short PreloadSettings { get; set; }
    public short MemberMin { get; set; }
    public short MemberCount { get; set; }
    public int Id { get; set; }
}