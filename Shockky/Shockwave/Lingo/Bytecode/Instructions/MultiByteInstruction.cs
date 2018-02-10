using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public abstract class MultiByteInstruction : Instruction
    {
        protected int _value;

        protected MultiByteInstruction(ShockwaveReader input, LingoHandler handler, OPCode op, bool isMulti)
            : base(op, handler)
        {
            _value = isMulti ?
                input.ReadBigEndian<short>() : input.ReadByte();
        }
    }
}
