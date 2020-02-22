namespace Shockky.Lingo.Instructions
{
    public class GreaterThanIns : Computation
    {
        public GreaterThanIns()
            : base(OPCode.GreaterThan, BinaryOperatorKind.GreaterThan)
        { }

        protected override object Execute(dynamic left, dynamic right) => (left > right);
    }
}