using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class MultipleIns : Computation
    {
        public MultipleIns(LingoHandler handler)
            : base(OPCode.Multiple, handler, "*")
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return (left * right);
        }*/
    }
}
