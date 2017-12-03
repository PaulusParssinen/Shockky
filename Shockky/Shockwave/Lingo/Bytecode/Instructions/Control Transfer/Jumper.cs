using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Control_Transfer
{
    public class Jumper : Instruction
    {
        public int Offset { get; }

        public Jumper(OPCode op, ShockwaveReader input, LingoHandler handler)
            : base(op, handler)
        {
            Offset = input.ReadInt16(true);
        }
    }
}
