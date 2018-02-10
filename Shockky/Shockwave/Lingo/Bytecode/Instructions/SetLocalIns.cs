using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetLocalIns : AssignmentInstruction
    {
        public override string Name
            => null; //Handler.Locals[_variableIndex];

        public SetLocalIns(ShockwaveReader input, LingoHandler handler, byte opByte) 
            : base(OPCode.SetLocal, opByte, input, handler)
        { }
    }
}
