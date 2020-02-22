namespace Shockky.Lingo.Instructions
{
    public class GreaterEqualsIns : Computation
    {
        public GreaterEqualsIns()
            : base(OPCode.GreaterEquals, BinaryOperatorKind.GreaterThanOrEqual)
        { }

        protected override object Execute(dynamic left, dynamic right) => (left >= right);
    }
}