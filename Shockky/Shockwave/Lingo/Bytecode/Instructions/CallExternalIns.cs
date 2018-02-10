using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallExternalIns : CallInstruction
    {
        public override string Function
            => null; //Handler.NameList[_functionNameIndex];

        public CallExternalIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.CallExternal, opByte > 0x80, input, handler)
        { }
    }
}
