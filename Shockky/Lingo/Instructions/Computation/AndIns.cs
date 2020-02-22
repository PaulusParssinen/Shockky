namespace Shockky.Lingo.Instructions
{
    public class AndIns : Computation
    {
        public AndIns()
            : base(OPCode.And, BinaryOperatorKind.And)
        { }

        protected override object Execute(dynamic left, dynamic right) => (left && right);
    }
}