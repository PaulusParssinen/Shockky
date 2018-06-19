using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class JoinStringIns : Instruction
    {
        public JoinStringIns()
            : base(OPCode.JoinString)
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
