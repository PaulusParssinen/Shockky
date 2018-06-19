using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetPropertyIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[NameIndex];

        public GetPropertyIns(LingoHandler handler)
            : base(OPCode.GetProperty, handler)
        { }
        public GetPropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            Value = propertyNameIndex;
        }
        public GetPropertyIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.GetProperty, handler, input, opByte)
        { }

        public override int GetPushCount() => 1;
    }
}
