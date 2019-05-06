namespace Shockky.Lingo.Instructions
{
    public class NotEqualIns : Computation
    {
        public NotEqualIns() //<>
            : base(OPCode.NotEqual, BinaryOperatorKind.InEquality)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left != right);
        }
    }
}