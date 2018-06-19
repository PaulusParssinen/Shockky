using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallLocalIns : Instruction
    {
        public int LocalHandlerIndex { get; set; }
        public LingoHandler LocalHandler => Pool.Handlers[LocalHandlerIndex];

        public CallLocalIns(LingoHandler handler)
            : base(OPCode.CallLocal, handler)
        { }
        public CallLocalIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.CallLocal, handler, input, opByte)
        { }
    }
}
