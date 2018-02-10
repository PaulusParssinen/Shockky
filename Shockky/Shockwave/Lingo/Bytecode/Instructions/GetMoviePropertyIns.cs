using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetMoviePropertyIns : MultiByteInstruction
    {
        public string Property
            => null; //Handler.NameList[_value];

        public GetMoviePropertyIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(input, handler, OPCode.GetMovieProp, opByte > 0x80)
        { }
    }
}
