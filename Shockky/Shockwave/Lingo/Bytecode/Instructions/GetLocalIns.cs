using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetLocalIns : Instruction
    {
        public int NameIndex => Value;
        public string Name => Pool.NameList[Handler.Locals[NameIndex]];
        
        public GetLocalIns(LingoHandler handler)
            : base(OPCode.GetLocal, handler)
        { }

        public GetLocalIns(LingoHandler handler, int localNameIndex)
            : this(handler)
        {
            //TODO: Implement this shit
            //Handler.Locals.Add, also adjust its index to be under int16 in namelist
        }

        public GetLocalIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.GetLocal, handler, input, opByte)
        { }

        public override int GetPopCount() => 1;
    }
}
