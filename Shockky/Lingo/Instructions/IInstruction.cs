using Shockky.IO;

namespace Shockky.Lingo.Instructions;

/// <summary>
/// Represents an lingo instruction with an optional immediate value.
/// </summary>
public partial interface IInstruction
{
    /// <summary>
    /// The <see cref="OPCode"/> value representing the instruction.
    /// </summary>
    OPCode OP { get; }

    /// <summary>
    /// The immediate operand encoded in the instruction.
    /// </summary>
    int Immediate { get; set; }

    int GetSize();
    void WriteTo(ShockwaveWriter output);
}