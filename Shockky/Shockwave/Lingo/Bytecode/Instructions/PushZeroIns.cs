using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class PushZeroIns : Instruction
    {
        public PushZeroIns(LingoHandler handler)
            : base(OPCode.PushInt0, handler)
        { }

        public override int GetPushCount()
        {
            return 1;
        }

        public override string ToString()
            => "0";
    }
}
