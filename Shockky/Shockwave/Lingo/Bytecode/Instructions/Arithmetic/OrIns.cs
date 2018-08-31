namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class OrIns : Computation
    {
        public OrIns()
            : base(OPCode.Or)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left || right); //TODO: Expressions?
        }
    }
}