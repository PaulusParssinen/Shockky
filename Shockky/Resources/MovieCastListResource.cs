using Shockky.IO;

namespace Shockky.Resources;

/// <summary>
/// The list of cast libraries used by a movie.
/// </summary>
[ShockwaveItem(BigEndian = true, IgnoreContainerEndianness = true, GenerateSerialization = true)]
public sealed partial class MovieCastListResource : IResource, IShockwaveItem
{
    public OsType Kind => OsType.MCsL;

    [ShockwaveItem(SizePrefixed = true, GenerateSerialization = true)]
    public sealed partial class HeaderData
    {
        public short Field4 { get; set; }
        public short Count { get; set; }
        public short EntriesPerCast { get; set; }
        public short FieldA { get; set; }
    }

    public required HeaderData Header { get; set; }
}