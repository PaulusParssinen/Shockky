using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetParameterIns : VariableReference
    {
        public override string Name 
            => Handler.Arguments[_variableIndex];

        public GetParameterIns(ShockwaveReader input, LingoHandler handler, byte opByte) 
            : base(OPCode.GetParameter, opByte, input, handler)
        { }
    }
}
