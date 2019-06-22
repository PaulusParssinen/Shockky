namespace Shockky.Lingo.Instructions
{
    public class SetMoviePropertryIns : VariableAssignment
    {
        private int _nameIndex;
        public int NameIndex
        {
            get => _nameIndex;
            set
            {
                Value = value;
                Name = Pool.NameList[value];
            }
        }

        public SetMoviePropertryIns(LingoHandler handler)
            : base(OPCode.SetMovieProp, handler)
        {
            IsMoviePropertyReference = true;
        }
        public SetMoviePropertryIns(LingoHandler handler, int moviePropertyIndex)
            : this(handler)
        {
            NameIndex = moviePropertyIndex;
        }
    }
}