using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SetMoviePropertryIns : Instruction
    {
        //TODO

        public SetMoviePropertryIns(LingoHandler handler)
            : base(OPCode.SetMovieProp, handler)
        { }
        public SetMoviePropertryIns(LingoHandler handler, int moviePropertyIndex)
            : this(handler)
        { }
        public SetMoviePropertryIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.SetMovieProp, handler, input, opByte)
        { }

        public override int GetPopCount() => 1;
    }
}
