using Shockky.IO;

namespace Shockky.Resources.Cast;

// TODO: Generalize VList parsing logic
[ShockwaveItem]
public sealed partial class CastMemberMetadata : IResource, IShockwaveItem
{
    public OsType Kind => OsType.VWCI;

    [Header]
    public sealed partial class MetadataHeader
    {
        [PadBefore(4)] // Skip garbage script pointer
        public int LegacyFlags { get; set; }

        public CastMemberInfoFlags Flags { get; set; }

        /// <summary>
        /// The Lingo script number for this cast member in 
        /// the cast library’s Lingo environment.
        /// </summary>
        [Condition("headerSize >= 20")]
        public int? ScriptContextNum { get; set; }
    }

    /// <summary>Header section.</summary>
    public MetadataHeader Header { get; set; } = null!;

    /// <summary>Offset table for entries.</summary>
    [OffsetTable]
    private OffsetTable Offsets { get; set; }

    [Entry(0), ParseStringAs(StringParseKind.FixedBytes)]
    public string? ScriptText { get; set; }
    [Entry(1), ParseStringAs(StringParseKind.PString)]
    public string? Name { get; set; }

    [Entry(2), ParseStringAs(StringParseKind.FixedBytes)]
    public string? FilePath { get; set; }
    [Entry(3), ParseStringAs(StringParseKind.FixedBytes)]
    public string? FileName { get; set; }
    [Entry(4), ParseStringAs(StringParseKind.FixedBytes)]
    public string? FileType { get; set; }

    // TODO: 5 = string, prop 44, script related?

    [Entry(7), ParseStringAs(StringParseKind.FixedBytes)]
    public string? UnknownProp45 { get; set; }

    // TODO: [Entry(9)]
    // public Guid? XtraGUID { get; set; }

    [Entry(10), ParseStringAs(StringParseKind.CString)]
    public string? XtraName { get; set; }

    // TODO: [Entry(12)]
    // public int[]? RegistrationPoints { get; set; }

    // TODO: 15 - MoA ID?

    [Entry(16), ParseStringAs(StringParseKind.FixedBytes)]
    public string? ClipboardFormat { get; set; }

    [Entry(17)]
    public int? CreationDate { get; set; }

    [Entry(18)]
    public int? ModifiedDate { get; set; }

    [Entry(19), ParseStringAs(StringParseKind.CString)]
    public string? ModifiedBy { get; set; }

    [Entry(20), ParseStringAs(StringParseKind.FixedBytes)]
    public string? Comments { get; set; }

    // TODO: 21
    // ReadOnlySpan<byte> imageFlags = input.ReadBytes(length); //4
    // 
    // ImageCompression = imageFlags[0] >> 4;
    // ImageQuality = imageFlags[1];

    public int GetBodySize(WriterOptions options)
    {
        throw new NotImplementedException();
    }
    public void WriteTo(ShockwaveWriter output, WriterOptions options)
    {
        throw new NotImplementedException();
    }
}