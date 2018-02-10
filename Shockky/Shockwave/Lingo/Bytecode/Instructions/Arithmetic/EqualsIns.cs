using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class EqualsIns : Computation
    {
        public EqualsIns(LingoHandler handler)
            : base(OPCode.Equals, handler, "=")
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return (left == right);
        }*/
    }
}
