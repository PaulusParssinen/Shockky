namespace Shockky;

/// <summary>
/// Marks a partial type for source-generated binary serialization.
/// The generator reads and writes properties in declaration order.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class ShockwaveItemAttribute : Attribute
{
    /// <summary>
    /// Whether to generate big-endian primitive reads and writes.
    /// </summary>
    public bool BigEndian { get; init; } = true;

    /// <summary>
    /// Whether generated reads should ignore the containing RIFF endianness flag.
    /// </summary>
    public bool IgnoreContainerEndianness { get; init; }

    /// <summary>
    /// Whether to generate <see cref="IShockwaveItem.GetBodySize"/> and <see cref="IShockwaveItem.WriteTo"/> implementations.
    /// </summary>
    public bool GenerateSerialization { get; init; }

    /// <summary>
    /// Whether the body is prefixed with a 4-byte size field.
    /// When true, the generated constructor accepts a <c>bodySize</c> parameter that conditions can reference.
    /// </summary>
    public bool SizePrefixed { get; init; }

    /// <summary>
    /// Expected body size values for debug validation.
    /// </summary>
    public int[] ExpectedSizes { get; init; } = [];
}