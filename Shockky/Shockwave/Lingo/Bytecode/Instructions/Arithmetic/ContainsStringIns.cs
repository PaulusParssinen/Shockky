using System;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class ContainsStringIns : Computation
    {
        public ContainsStringIns(LingoHandler handler)
            : base(OPCode.ContainsString, handler, "contains")
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            string lhs = left;
            string rhs = right;
            return lhs.IndexOf(rhs, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
