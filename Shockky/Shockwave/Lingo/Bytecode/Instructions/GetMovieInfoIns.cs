using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetMovieInfoIns : VariableReference
    {
        public override string Name
            => Handler.NameList[_variableIndex];

        public GetMovieInfoIns(ShockwaveReader input, LingoHandler handler, byte opByte) 
            : base(OPCode.GetMovieInfo, opByte, input, handler)
        { }
    }
}
