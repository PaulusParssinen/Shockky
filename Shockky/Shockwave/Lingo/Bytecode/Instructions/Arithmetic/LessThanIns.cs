using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class LessThanIns : Computation
    {
        public LessThanIns(LingoHandler handler)
            : base(OPCode.LessThan, handler, "<")
        { }

      /*  protected override object Execute(dynamic left, dynamic right)
        {
            return (left < right);
        }*/
    }
}
