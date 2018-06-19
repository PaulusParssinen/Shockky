using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallExternalIns : Instruction
    {
        public int ExternalFunctionNameIndex { get; set; }
        public string ExternalFunctionName => Pool.NameList[ExternalFunctionNameIndex];

        public CallExternalIns(LingoHandler handler)
            : base(OPCode.CallExternal, handler)
        { }
        public CallExternalIns(LingoHandler handler, int externalFunctionNameIndex)
            : this(handler)
        {
            ExternalFunctionNameIndex = externalFunctionNameIndex;
        }

        public CallExternalIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.CallExternal, handler, input, opByte)
        {
            ExternalFunctionNameIndex = Value;
        } //6Y8-4611
    }
}
