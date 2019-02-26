namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GetMovieInfoIns : Instruction
    {
        private string _value;
        public new string Value
        {
            get => _value;
            set
            {
                _value = value;
                _valueIndex = Pool.AddName(value);
            }
        }

        private int _valueIndex;
        public int ValueIndex
        {
            get => _valueIndex;
            set
            {
                base.Value = value;
                _valueIndex = value;
                _value = Pool.NameList[value];
            }
        }

        public GetMovieInfoIns(LingoHandler handler)
            : base(OPCode.GetMovieInfo, handler)
        { }
        public GetMovieInfoIns(LingoHandler handler, int nameIndex)
            : this(handler)
        {
            ValueIndex = nameIndex;
        }
        public GetMovieInfoIns(LingoHandler handler, string name)
            : this(handler)
        {
            Value = name;
        }
    }
}