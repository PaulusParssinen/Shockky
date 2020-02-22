namespace Shockky.Lingo.Instructions
{
    public class JoinPadStringIns : Computation
    {
        public JoinPadStringIns() 
            : base(OPCode.JoinPadString, BinaryOperatorKind.JoinPadString)
        { }

        protected override object Execute(dynamic left, dynamic right) => left + " " + right;
    }
}