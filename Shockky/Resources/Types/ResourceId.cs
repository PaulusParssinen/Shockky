namespace Shockky.Resources.Types;

/// <summary>
/// Represents an resource identifier consisting of <see cref="OsType"/> and a numeric <paramref name="Id"/> used <see cref="IResource"/> lookups.
/// </summary>
public readonly record struct ResourceId(OsType Kind, int Id);