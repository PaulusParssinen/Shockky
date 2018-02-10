using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetLocalIns : VariableReference
    {
        public override string Name
            => null; //Handler.Locals[_variableIndex];

        public GetLocalIns(ShockwaveReader input, LingoHandler handler, byte opByte) 
            : base(OPCode.GetLocal, opByte, input, handler)
        { }
    }
}
