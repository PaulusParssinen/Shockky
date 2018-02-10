using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallObjectIns : CallInstruction
    {


        public override string Function
            => null; //Handler.NameList[_functionNameIndex];

        public CallObjectIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.CallObj, opByte > 0x80, input, handler)
        { }
    }
}
