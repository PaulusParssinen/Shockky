using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class PushIntIns : MultiByteInstruction
    {
        public int Value { get; set; }

        public PushIntIns(ShockwaveReader input, LingoHandler handler, bool isMulti)
            : base(input, handler, OPCode.PushInt, isMulti)
        {
            Value = _value;
        }

        public override string ToString()
            => Value.ToString();
    }
}
