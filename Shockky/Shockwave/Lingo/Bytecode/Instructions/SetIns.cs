using Shockky.IO;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetIns : Instruction
    {
        public SetIns(LingoHandler handler, ShockwaveReader input, byte opByte) 
            : base(OPCode.Set, handler, input, opByte)
        {
            //int id = input.ReadByte();
        }
    }
}