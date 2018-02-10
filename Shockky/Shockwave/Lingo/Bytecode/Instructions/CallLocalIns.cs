using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallLocalIns : CallInstruction
    {
        public override string Function
            => null; //Handler.Script.Handlers[_functionNameIndex].Name;

        public CallLocalIns(ShockwaveReader input, LingoHandler handler) 
            : base(OPCode.CallLocal, false, input, handler)
        { }
    }
}
