using Shockky.SourceGeneration.Helpers;

namespace Shockky.SourceGeneration.Models;

/// <summary>
/// Model representing a type annotated with [ShockwaveItem].
/// </summary>
internal sealed record ShockwaveItemInfo(
    HierarchyInfo Hierarchy,
    bool BigEndian,
    bool IgnoreContainerEndianness,
    bool GenerateSerialization,
    bool SizePrefixed,
    EquatableArray<int> ExpectedSizes,
    EquatableArray<PropertySerializationInfo> Properties,
    int? MaxEntryIndex,
    bool HasExistingGetBodySize,
    bool HasExistingWriteTo) : IEquatable<ShockwaveItemInfo?>;

/// <summary>
/// Model for a property to be serialized.
/// </summary>
internal sealed record PropertySerializationInfo(
    string Name,
    string TypeFullName,
    int PadBefore,
    int PadAfter,
    string? Condition,
    ParseKind ParseKind,
    bool IsNullable,
    PropertyKind Kind,
    int EntryIndex,
    TypeSerializationKind SerializationKind,
    string? EnumUnderlyingTypeFullName,
    bool IsSizePrefixed) : IEquatable<PropertySerializationInfo>;

/// <summary>
/// Serializable type shape for generated binary I/O.
/// </summary>
internal enum TypeSerializationKind
{
    Unsupported,
    Byte,
    SByte,
    Boolean,
    Int16,
    UInt16,
    Int32,
    UInt32,
    UInt64,
    Double,
    String,
    Enum,
    ShockwaveItem,
}

/// <summary>
/// Kind of property in the read sequence.
/// </summary>
internal enum PropertyKind
{
    /// <summary>Regular sequential property.</summary>
    Sequential,
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