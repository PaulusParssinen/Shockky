using System;

namespace Shockky.Lingo.Bytecode.Instructions
{
    public class ContainsStringIns : Computation
    {
        public ContainsStringIns()
            : base(OPCode.ContainsString, BinaryOperatorKind.ContainsString)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            string lhs = left;
            string rhs = right;
            return lhs.IndexOf(rhs, StringComparison.InvariantCultureIgnoreCase) != -1;
        }
    }
}