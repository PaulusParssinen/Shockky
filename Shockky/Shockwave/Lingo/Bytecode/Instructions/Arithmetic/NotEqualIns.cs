using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class NotEqualIns : Computation
    {
        public NotEqualIns(LingoHandler handler)
            : base(OPCode.NotEqual, handler, "<>")
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left != right);
        }
    }
}
