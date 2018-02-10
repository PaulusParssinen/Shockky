using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetGlobalIns : MultiByteInstruction
    {
        public string Global
            => null; //Handler.NameList[_value];

        public SetGlobalIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(input, handler, OPCode.SetGlobal, opByte > 0x80)
        { }
    }
}
