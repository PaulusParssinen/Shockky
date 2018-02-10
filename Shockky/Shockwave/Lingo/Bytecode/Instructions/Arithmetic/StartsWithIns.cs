using System;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class StartsWithIns : Computation
    {
        public StartsWithIns(LingoHandler handler) 
            : base(OPCode.Contains0String, handler, "starts")
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            string lfs = left;
            string rhs = right;
            return lfs.StartsWith(rhs, StringComparison.InvariantCultureIgnoreCase);
        }*/
    }
}
