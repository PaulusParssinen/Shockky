namespace Shockky;

/// <summary>
/// The containing type should also have an <see cref="OffsetTableAttribute"/> property.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
internal sealed class HeaderAttribute : Attribute
{
    /// <summary>
    /// Expected header size values for validation.
    /// </summary>
    public int[] ExpectedSizes { get; init; } = [];
}