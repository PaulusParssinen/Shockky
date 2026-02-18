namespace Shockky.Lingo.Instructions;

public enum ImmediateKind
{
    None = 0,
    Integer,
    Literal,
    NameIndex,
    ScriptIndex,
    Offset,
    NegativeOffset,
    ArgumentIndex,
    LocalIndex
}