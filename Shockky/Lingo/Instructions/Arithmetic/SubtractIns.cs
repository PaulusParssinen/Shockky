namespace Shockky.Lingo.Bytecode.Instructions
{
    public class SubtractIns : Computation
    {
        public SubtractIns() 
            : base(OPCode.Substract, BinaryOperatorKind.Subtract)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left - right);
        }
    }
}