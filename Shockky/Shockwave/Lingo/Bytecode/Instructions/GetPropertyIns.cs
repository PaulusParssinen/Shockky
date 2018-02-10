using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetPropertyIns : VariableReference
    {
        public override string Name
            => null; //Handler.Arguments[_variableIndex];

        public GetPropertyIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.GetProperty, opByte, input, handler)
        { }
    }
}
