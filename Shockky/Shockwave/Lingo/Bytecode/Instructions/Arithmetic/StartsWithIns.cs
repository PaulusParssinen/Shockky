using System;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class StartsWithIns : Computation
    {
        public StartsWithIns() 
            : base(OPCode.Contains0String)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            string lhs = left;
            string rhs = right;
            return lhs.StartsWith(rhs, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}