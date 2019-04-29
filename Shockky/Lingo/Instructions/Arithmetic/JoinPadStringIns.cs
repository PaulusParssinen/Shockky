namespace Shockky.Lingo.Bytecode.Instructions
{
    public class JoinPadStringIns : Computation
    {
        public JoinPadStringIns() 
            : base(OPCode.JoinPadString)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return left + " " + right; //absolutely fucking top tier
        }
    }
}