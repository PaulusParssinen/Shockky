namespace Shockky.SourceGeneration.Models;

/// <summary>
/// A model representing all necessary info for full generation of instruction definition.
/// </summary>
internal sealed partial record InstructionInfo(
    string Name,
    TypedConstantInfo OpValue,
    ImmediateKind ImmediateKind);

internal enum ImmediateKind : int
{
    None = 0,
    Integer,
    Literal,
    NameIndex,
    ScriptIndex,
    Offset,
    NegativeOffset,
    ArgumentIndex,
    LocalVarIndex
}