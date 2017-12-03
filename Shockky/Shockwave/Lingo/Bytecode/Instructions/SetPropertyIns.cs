using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetPropertyIns : AssignmentInstruction
    {
        public override string Name
            => Handler.NameList[_variableIndex];

        public SetPropertyIns(ShockwaveReader input, LingoHandler handler, byte opByte) 
            : base(OPCode.SetProperty, opByte, input, handler)
        { }
    }
}
