using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class WrapListIns : Instruction
    {
        public WrapListIns(LingoHandler handler) 
            : base(OPCode.WrapList, handler)
        { }

        public override int GetPopCount()
        {
            return 1;
        }

        public override int GetPushCount()
        {
            return 1;
        }
    }
}
