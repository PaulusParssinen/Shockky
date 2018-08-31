using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Stack_Management;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class PushIntIns : Primitive //TODO: OPCode.PushInt2
    {
        public PushIntIns(LingoHandler handler)
            : base(OPCode.PushInt, handler)
        { }

        public PushIntIns(LingoHandler handler, int value)
            : this(handler)
        {
            Value = value;
        }

        public PushIntIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.PushInt, handler, input, opByte)
        { }
    }
}