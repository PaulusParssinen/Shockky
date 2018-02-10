using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetObjPropertyIns : VariableReference
    {
        public override string Name
            => null; //Handler.NameList[_variableIndex];

        public GetObjPropertyIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.GetObjProp, opByte, input, handler)
        { }
    }
}
