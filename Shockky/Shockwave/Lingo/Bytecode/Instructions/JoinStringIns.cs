using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class JoinStringIns : Instruction
    {
        public JoinStringIns(LingoHandler handler)
            : base(OPCode.JoinString, handler)
        { }

        public override int GetPopCount()
        {
            return 2;
        }

        public override int GetPushCount()
        {
            return 1;
        }
    }
}
