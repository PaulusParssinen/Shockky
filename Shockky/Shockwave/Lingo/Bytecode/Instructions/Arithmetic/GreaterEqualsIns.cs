using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class GreaterEqualsIns : Computation
    {
        public GreaterEqualsIns(LingoHandler handler)
            : base(OPCode.GreaterEquals, handler, ">=")
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left >= right);
        }
    }
}