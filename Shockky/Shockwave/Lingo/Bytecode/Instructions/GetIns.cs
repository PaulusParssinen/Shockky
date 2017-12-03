using System.Diagnostics;
using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetIns : Instruction
    {
        public GetIns(ShockwaveReader input, LingoHandler handler) 
            : base(OPCode.Get, handler)
        {
            int id = input.ReadByte();
            Debug.WriteLine("TODO GET INSTRUCTION ID: " + id);
        }
    }
}
