namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SetMoviePropertryIns : Instruction
    {
        //TODO

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