namespace Shockky.Resources.Cast;

/// <summary>
/// Metadata for external cast libraries (Cinf chunk).
/// </summary>
[ShockwaveItem(BigEndian = true, IgnoreContainerEndianness = true, GenerateSerialization = true)]
public sealed partial class CastMetadataResource : IResource, IShockwaveItem
{
    public OsType Kind => OsType.Cinf;

    [ShockwaveItem(SizePrefixed = true, GenerateSerialization = true, ExpectedSizes = [4])]
    public sealed partial class HeaderData;

    public HeaderData Header { get; set; } = new();

    /// <summary>
    /// For an external cast, the original file path.
    /// </summary>
    [Entry(3), ParseStringAs(StringParseKind.PString)]
    public string? FilePath { get; set; }
}