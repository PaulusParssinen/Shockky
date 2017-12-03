using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Control_Transfer
{
    public class EndRepeatIns : Jumper
    {
        public EndRepeatIns(ShockwaveReader input, LingoHandler handler) 
            : base(OPCode.EndRepeat, input, handler)
        { }
    }
}
