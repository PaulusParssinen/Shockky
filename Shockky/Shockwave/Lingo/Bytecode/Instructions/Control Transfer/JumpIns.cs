using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Control_Transfer
{
    public class JumpIns : Jumper
    {
        public JumpIns(ShockwaveReader input, LingoHandler handler)
            : base(OPCode.Jump, input, handler)
        { }
    }
}
