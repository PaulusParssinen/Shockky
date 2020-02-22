namespace Shockky.Lingo.Instructions
{
    public class SubtractIns : Computation
    {
        public SubtractIns() 
            : base(OPCode.Substract, BinaryOperatorKind.Subtract)
        { }

        protected override object Execute(dynamic left, dynamic right) => (left - right);
    }
}