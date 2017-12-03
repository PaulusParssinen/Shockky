using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetIns : Instruction
    {
        public SetIns(ShockwaveReader input, LingoHandler handler) 
            : base(OPCode.Set, handler)
        {
            int id = input.ReadByte();
            Debug.WriteLine("TODO SET INSTRUCTION ID: " + id);
        }
    }
}
