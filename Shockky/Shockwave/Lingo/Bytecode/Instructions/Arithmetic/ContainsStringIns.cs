using System;
namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class ContainsStringIns : Computation
    {
        public ContainsStringIns()
            : base(OPCode.ContainsString)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            string lhs = left;
            string rhs = right;
            return lhs.IndexOf(rhs, StringComparison.InvariantCultureIgnoreCase) != -1;
        }
    }
}