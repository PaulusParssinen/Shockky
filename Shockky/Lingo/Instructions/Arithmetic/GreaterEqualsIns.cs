namespace Shockky.Lingo.Bytecode.Instructions
{
    public class GreaterEqualsIns : Computation
    {
        public GreaterEqualsIns()
            : base(OPCode.GreaterEquals, BinaryOperatorKind.GreaterThanOrEqual)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left >= right);
        }
    }
}