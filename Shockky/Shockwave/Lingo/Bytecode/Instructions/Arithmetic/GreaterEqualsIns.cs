namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GreaterEqualsIns : Computation
    {
        public GreaterEqualsIns()
            : base(OPCode.GreaterEquals)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left >= right);
        }
    }
}