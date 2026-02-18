namespace Shockky.Lingo.Instructions;

/// <summary>
/// Instructs Shockky source generator to generate source to serialize and deserialize instances
/// of the Lingo instruction.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class OPAttribute() : Attribute
{
    public OPAttribute(ImmediateKind kind)
        : this()
    { }
}