using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class PushConstantIns : VariableReference
    {
        public override string Name
            => null; //Handler.Script.Literals[_variableIndex].Value;

        public PushConstantIns(ShockwaveReader input, LingoHandler handler, byte opByte) 
            : base(OPCode.PushConstant, opByte, input, handler)
        { }
    }
}
