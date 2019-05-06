namespace Shockky.Lingo.Instructions
{
    public class EqualsIns : Computation
    {
        public EqualsIns()
            : base(OPCode.Equals, BinaryOperatorKind.Equality)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left == right);
        }
    }
}