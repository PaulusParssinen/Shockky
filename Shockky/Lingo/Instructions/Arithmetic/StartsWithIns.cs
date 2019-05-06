using System;

namespace Shockky.Lingo.Instructions
{
    public class StartsWithIns : Computation
    {
        public StartsWithIns() 
            : base(OPCode.StartsWith, BinaryOperatorKind.StartsWith)
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            string lhs = left;
            string rhs = right;
            return lhs.StartsWith(rhs, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}