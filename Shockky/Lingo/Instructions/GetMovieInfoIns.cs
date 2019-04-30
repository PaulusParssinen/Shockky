namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetMovieInfoIns : VariableReference
    {
        private int _valueIndex;
        public int ValueIndex
        {
            get => _valueIndex;
            set
            {
                _valueIndex = value;
                Name = Pool.NameList[value];
            }
        }

        public GetMovieInfoIns(LingoHandler handler)
            : base(OPCode.GetMovieInfo, handler)
        {
            IsMovieReference = true;
        }
        public GetMovieInfoIns(LingoHandler handler, int nameIndex)
            : this(handler)
        {
            ValueIndex = nameIndex;
        }
        public GetMovieInfoIns(LingoHandler handler, string name)
            : this(handler)
        {
            Name = name;
        }

        public override int GetPopCount() => 1;
    }
}