using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CastStringIns : Instruction
    {
        public CastStringIns(LingoHandler handler) 
            : base(OPCode.CastString, handler)
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
