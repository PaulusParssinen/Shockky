using Shockky.IO;

namespace Shockky.Resources;

/// <summary>
/// Information about a movie file (VWFI chunk).
/// </summary>
[ShockwaveItem(GenerateSerialization = true, IgnoreContainerEndianness = true)]
public sealed partial class FileInfoResource : IShockwaveItem, IResource
{
    public OsType Kind => OsType.VWFI;

    /// <summary>
    /// The header section, containing fixed-size fields.
    /// </summary>
    [ShockwaveItem(SizePrefixed = true, GenerateSerialization = true, ExpectedSizes = [16, 20])]
    public sealed partial class HeaderData
    {
        /// <summary>
        /// The kinds of event handlers in the movie script. Not used by D4+.
        /// </summary>
        [PadBefore(4)] // Skip garbage script pointer
        public MovieEventHandlers EventHandlers { get; set; }

        /// <summary>
        /// Movie flags that need to be serialized to disk.
        /// </summary>
        public FileInfoFlags Flags { get; set; }

        /// <summary>
        /// Script context number.
        /// </summary>
        [Condition("bodySize >= 20")]
        public int? ScriptContextNum { get; set; }
    }

    /// <summary>Header section.</summary>
    public required HeaderData Header { get; set; }

    /// <summary>The movie script text. Not used by D4+.</summary>
    [Entry(0), ParseStringAs(StringParseKind.FixedBytes)]
    public string? MovieScriptText { get; set; }

    /// <summary>The name of the user who created the file.</summary>
    [Entry(1), ParseStringAs(StringParseKind.PString)]
    public string? CreatedBy { get; set; }

    /// <summary>The name of the user who last modified the file.</summary>
    [Entry(2), ParseStringAs(StringParseKind.PString)]
    public string? ModifiedBy { get; set; }

    /// <summary>The original path of the file.</summary>
    [Entry(3), ParseStringAs(StringParseKind.PString)]
    public string? FilePath { get; set; }

    /// <summary>The preload strategy for the cast.</summary>
    [Entry(4)]
    public CastPreloadStrategy? Preload { get; set; }

    /// <summary>Shared cast library number.</summary>
    [Entry(5)]
    public short? SharedCastLibNum { get; set; }

    [Entry(6)]
    public short? OldSharedMinCast { get; set; }

    [Entry(7)]
    public short? NewSharedMinCast { get; set; }

}