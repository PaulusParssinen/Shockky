using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class NewPropListIns : Instruction
    {
        public NewPropListIns(LingoHandler handler) 
            : base(OPCode.NewPropList, handler)
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
