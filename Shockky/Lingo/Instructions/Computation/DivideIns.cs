namespace Shockky.Lingo.Instructions
{
    public class DivideIns : Computation
    {
        public DivideIns()
            : base(OPCode.Divide, BinaryOperatorKind.Divide)
        { }

        protected override object Execute(dynamic left, dynamic right) => (left / right);
    }
}