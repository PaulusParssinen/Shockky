namespace Shockky.Lingo.Instructions
{
    public class GetMoviePropertyIns : VariableReference
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

        public GetMoviePropertyIns(LingoHandler handler)
            : base(OPCode.GetMovieProp, handler)
        {
            IsMoviePropertyReference = true;
        }
        public GetMoviePropertyIns(LingoHandler handler, int propertyNameIndex)
            : this(handler)
        {
            ValueIndex = propertyNameIndex;
        }
        public GetMoviePropertyIns(LingoHandler handler, string moviePropertyName)
            : this(handler)
        {
            Name = moviePropertyName;
        }
    }
}