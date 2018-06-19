using System;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class StartsWithIns : Computation
    {
        public StartsWithIns() 
            : base(OPCode.Contains0String)
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            string lfs = left;
            string rhs = right;
            return lfs.StartsWith(rhs, StringComparison.InvariantCultureIgnoreCase);
        }*/
    }
}
