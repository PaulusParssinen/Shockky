using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class ReturnIns : Instruction
    {
        public ReturnIns(LingoHandler handler)
            : base(OPCode.Return, handler)
        { }
    }
}
