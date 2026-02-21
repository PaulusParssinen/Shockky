using Shockky.SourceGeneration.Helpers;

namespace Shockky.SourceGeneration.Models;

/// <summary>
/// Model representing a nested type annotated with [Header].
/// </summary>
internal sealed record HeaderInfo(
    HierarchyInfo Hierarchy,
    bool BigEndian,
    EquatableArray<int> ExpectedSizes,
    EquatableArray<PropertyReadInfo> Properties) : IEquatable<HeaderInfo?>;