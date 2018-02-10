using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class GreaterThanIns : Computation
    {
        public GreaterThanIns(LingoHandler handler)
            : base(OPCode.GreaterThan, handler, ">")
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return (left > right);
        }*/
    }
}