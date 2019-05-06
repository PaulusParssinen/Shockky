namespace Shockky.Lingo.Instructions
{
    public class MultipleIns : Computation
    {
        public MultipleIns()
            : base(OPCode.Multiple, BinaryOperatorKind.Multiply)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left * right);
        }
    }
}