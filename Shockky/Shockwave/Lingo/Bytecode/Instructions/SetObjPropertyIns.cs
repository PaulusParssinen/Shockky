using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetObjPropertyIns : AssignmentInstruction
    {
        public override string Name
            => Handler.NameList[_variableIndex];

        public SetObjPropertyIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.SetObjProp, opByte, input, handler)
        { }

        public override void Translate()
        {
            var value = Handler.Expressions.Pop();
            var obj = Handler.Expressions.Pop();
        }
    }
}
