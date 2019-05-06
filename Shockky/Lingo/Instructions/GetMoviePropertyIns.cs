using Shockky.IO;

namespace Shockky.Lingo.Instructions
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
                base.Value = value;
                _nameIndex = value;
                _name = Pool.NameList[value];
            }
        }

        public GetMoviePropertyIns(LingoHandler handler)
            : base(OPCode.GetMovieProp, handler)
        { }
        public GetMoviePropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            NameIndex = propertyNameIndex;
        }
        public GetMoviePropertyIns(LingoHandler handler, string moviePropertyName)
            : this(handler)
        {
            Name = moviePropertyName;
        }

        public override int GetPopCount() => 1;
    }
}