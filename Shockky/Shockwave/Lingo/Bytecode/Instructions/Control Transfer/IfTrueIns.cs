using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Control_Transfer
{
    public class IfTrueIns : Jumper
    {
        public IfTrueIns(ShockwaveReader input, LingoHandler handler)
            : base(OPCode.IfTrue, input, handler)
        {
            
        }
    }
}
