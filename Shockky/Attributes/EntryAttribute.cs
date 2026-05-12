namespace Shockky;

/// <summary>
/// Marks a property as a entry with the given index.
/// Requires an <see cref="OffsetTableAttribute"/> property in the same type.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class EntryAttribute(int index) : Attribute
{
    public int Index { get; } = index;
}