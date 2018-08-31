using Shockky.IO;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class CallObjectIns : Instruction
    {
        public int HandlerNameIndex => Value;
        public string HandlerName => Pool.NameList[HandlerNameIndex];

        public CallObjectIns(LingoHandler handler)
            : base(OPCode.CallObj, handler)
        { }
        public CallObjectIns(LingoHandler handler, int handlerNameIndex)
            : base(OPCode.CallObj, handler)
        {
            Value = handlerNameIndex;
        }
        public CallObjectIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.CallObj, handler, input, opByte)
        { }
    }
}