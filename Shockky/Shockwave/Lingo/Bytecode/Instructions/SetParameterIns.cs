using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetParameterIns : AssignmentInstruction
    {
        public override string Name
            => Handler.Arguments[_variableIndex];

        public SetParameterIns(ShockwaveReader input, LingoHandler handler, byte opByte) 
            : base(OPCode.SetParameter, opByte, input, handler)
        { }
    }
}
