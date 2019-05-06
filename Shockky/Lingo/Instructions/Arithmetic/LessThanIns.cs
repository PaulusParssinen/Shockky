namespace Shockky.Lingo.Instructions
{
    public class LessThanIns : Computation
    {
        public LessThanIns()
            : base(OPCode.LessThan, BinaryOperatorKind.LessThan)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left < right);
        }
    }
}