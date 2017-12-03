using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetMoviePropertryIns : AssignmentInstruction
    {
        public override string Name
            => "SETMOVIEPROPERTYINSTPODO";

        public SetMoviePropertryIns(ShockwaveReader input, LingoHandler handler, byte opByte)
            : base(OPCode.SetMovieProp, opByte, input, handler)
        { }
    }
}
