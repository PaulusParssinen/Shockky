using Shockky.IO;

namespace Shockky;

/// <summary>
/// Marks a property as the  <see cref="OffsetTable">offset table</see> for a sparse structures.
/// Properties marked with <see cref="EntryAttribute"/> will use this for indexed access.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class OffsetTableAttribute : Attribute;