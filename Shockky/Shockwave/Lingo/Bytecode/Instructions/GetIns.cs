using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetIns : Instruction
    {
        public GetIns(LingoHandler handler, ShockwaveReader input, byte opByte) 
            : base(OPCode.Get, handler, input, opByte)
        {
        }
    }
}
