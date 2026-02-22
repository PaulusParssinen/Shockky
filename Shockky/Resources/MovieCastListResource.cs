using Shockky.IO;

namespace Shockky.Resources;

/// <summary>
/// The list of cast libraries used by a movie.
/// </summary>
[ShockwaveItem(BigEndian = true)]
public sealed partial class MovieCastListResource : IResource, IShockwaveItem
{
    public OsType Kind => OsType.MCsL;

    [Header]
    public sealed partial class HeaderData
    {
        public short Field4 { get; set; }
        public short Count { get; set; }
        public short EntriesPerCast { get; set; }
        public short FieldA { get; set; }
    }

    public HeaderData Header { get; set; } = null!;

    [OffsetTable]
    private OffsetTable Offsets { get; set; }

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }

    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}
