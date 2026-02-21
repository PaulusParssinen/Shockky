namespace Shockky;

/// <summary>
/// Marks a partial type for source-generated binary deserialization.
/// The generator reads properties in declaration order.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class ShockwaveItemAttribute : Attribute
{
    /// <summary>
    /// Whether to generate big-endian reading.
    /// </summary>
    public bool BigEndian { get; init; } = true;
}