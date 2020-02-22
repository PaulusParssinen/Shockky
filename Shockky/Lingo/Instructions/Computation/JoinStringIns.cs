namespace Shockky.Lingo.Instructions
{
    public class JoinStringIns : Computation
    {
        public JoinStringIns()
            : base(OPCode.JoinString, BinaryOperatorKind.JoinString)
        { }

        protected override object Execute(dynamic left, dynamic right) => $"{left}{right}";
    }
}