using Shockky.SourceGeneration.Helpers;

namespace Shockky.SourceGeneration.Models;

/// <summary>
/// Model representing a type annotated with [ShockwaveItem].
/// </summary>
internal sealed record ShockwaveItemInfo(
    HierarchyInfo Hierarchy,
    bool BigEndian,
    EquatableArray<PropertyReadInfo> Properties,
    string? OffsetTablePropertyName,
    int? MaxEntryIndex) : IEquatable<ShockwaveItemInfo?>;

/// <summary>
/// Model for a property to be read.
/// </summary>
internal sealed record PropertyReadInfo(
    string Name,
    string TypeFullName,
    int PadBefore,
    int PadAfter,
    string? Condition,
    ParseKind ParseKind,
    bool IsNullable,
    PropertyKind Kind,
    int EntryIndex) : IEquatable<PropertyReadInfo>;

/// <summary>
/// Kind of property in the read sequence.
/// </summary>
internal enum PropertyKind
{
    /// <summary>Regular sequential property.</summary>
    Sequential,
    /// <summary>Nested type marked with [Header].</summary>
    Header,
    /// <summary>Offset table property.</summary>
    OffsetTable,
    /// <summary>Entry indexed by offset table.</summary>
    Entry,
}

internal enum ParseKind
{
    Default = 0,
    PString,
    CString,
    FixedBytes,
}