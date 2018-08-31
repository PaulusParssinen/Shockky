namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class AndIns : Computation
    {
        public AndIns()
            : base(OPCode.And)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left && right); //TODO: Expressions?
        }
    }
}