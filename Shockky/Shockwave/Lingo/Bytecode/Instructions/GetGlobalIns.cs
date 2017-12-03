using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetGlobalIns : VariableReference
    {
        public override string Name 
            => Handler.NameList[_variableIndex];

        public GetGlobalIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.GetGlobal, opByte, input, handler)
        { }
    }
}
