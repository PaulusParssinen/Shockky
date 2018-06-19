using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetParameterIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[Handler.Arguments[NameIndex]];

        public GetParameterIns(LingoHandler handler)
            : base(OPCode.GetParameter, handler)
        { }
        public GetParameterIns(LingoHandler handler, int argumentNameIndex)
            : this(handler)
        {
            //TODO: index under int16 in namelist with this one too
        }
        public GetParameterIns(LingoHandler handler, ShockwaveReader input, byte opByte) 
            : base(OPCode.GetParameter, handler, input, opByte)
        { }
        
        public override int GetPushCount() => 1;
    }
}
