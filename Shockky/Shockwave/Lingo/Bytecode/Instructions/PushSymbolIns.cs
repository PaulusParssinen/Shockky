using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class PushSymbolIns : VariableReference
    {
        public override string Name
            => Handler.NameList[_variableIndex];

        public PushSymbolIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.PushSymbol, opByte, input, handler)
        { }

        public override string ToString()
            => "#" + Name;
    }
}
