//HintName: Shockky.Lingo.Instructions.IInstruction.Read.g.cs
namespace Shockky.Lingo.Instructions;

public partial interface IInstruction
{
    [global::System.CodeDom.Compiler.GeneratedCode("InstructionGenerator", "1.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCode]
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static IInstruction Read(ref global::Shockky.IO.ShockwaveReader input)
    {
        byte op = input.ReadByte();
        int immediate = op >> 6 switch 
        {
            1 => input.ReadByte(),
            2 => input.ReadInt16LittleEndian(),
            3 => input.ReadInt32LittleEndian(),
            _ => 0
        };

        if (op >= 0x80) op = (byte)(op & 0x3F | 0x40);
        return (OPCode)op switch
        {
            _ => throw null
        };
    }
}