using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class DivideIns : Computation
    {
        public DivideIns(LingoHandler handler)
            : base(OPCode.Divide, handler, "/")
        { }

      /*  protected override object Execute(dynamic left, dynamic right)
        {
            return (left / right);
        }*/
    }
}
