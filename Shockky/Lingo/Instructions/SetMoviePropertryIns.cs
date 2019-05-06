namespace Shockky.Lingo.Instructions
{
    public class SetMoviePropertryIns : VariableReference
    {
        private int _nameIndex;
        public int NameIndex
        {
            get => _nameIndex;
            set
            {
                Value = value;
                Name = Pool.NameList[Handler.Locals[Value]];
            }
        }

        public SetMoviePropertryIns(LingoHandler handler)
            : base(OPCode.SetMovieProp, handler)
        { }
        public SetMoviePropertryIns(LingoHandler handler, int moviePropertyIndex)
            : this(handler)
        {
            Value = moviePropertyIndex;
        }

        public override int GetPopCount() => 1;
    }
}