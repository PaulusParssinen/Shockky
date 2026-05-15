using System.Diagnostics;

namespace Shockky.Resources;

[DebuggerDisplay("{Kind} | Length: {Length}, Offset: {Offset}")]
[ShockwaveItem(BigEndian = true, GenerateSerialization = true)]
public sealed partial class ResourceEntry : IShockwaveItem
{
    public OsType Kind { get; set; }
    public int Length { get; set; }
    public int Offset { get; set; }
    public ResourceEntryFlags Flags { get; set; }
    public short Unknown { get; set; }
    public int Link { get; set; }
}