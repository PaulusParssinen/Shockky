using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetIns : Instruction
    {
        public SetIns(LingoHandler handler, ShockwaveReader input, byte opByte) 
            : base(OPCode.Set, handler, input, opByte)
        {
            //int id = input.ReadByte();
        }
    }
}
