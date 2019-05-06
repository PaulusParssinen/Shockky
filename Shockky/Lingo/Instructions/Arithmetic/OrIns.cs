namespace Shockky.Lingo.Instructions
{
    public class OrIns : Computation
    {
        public OrIns()
            : base(OPCode.Or, BinaryOperatorKind.Or)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left || right);
        }
    }
}