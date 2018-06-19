using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GetMoviePropertyIns : Instruction
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _nameIndex = Pool.AddName(value);
            }
        }

        private int _nameIndex;
        public int NameIndex
        {
            get => _nameIndex;
            set
            {
                _nameIndex = value;
                _name = Pool.NameList[Value];
            }
        }

        public GetMoviePropertyIns(LingoHandler handler)
            : base(OPCode.GetMovieProp, handler)
        { }
        public GetMoviePropertyIns(LingoHandler handler, string moviePropertyName)
            : this(handler)
        {
            Name = moviePropertyName;
        }
        public GetMoviePropertyIns(LingoHandler handler, ShockwaveReader input, byte opByte)
            : base(OPCode.GetMovieProp, handler, input, opByte)
        {
            NameIndex = Value;
        }

        public override int GetPopCount() => 1;
    }
}
