using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class DivideIns : Computation
    {
        public DivideIns()
            : base(OPCode.Divide)
        { }

      /*  protected override object Execute(dynamic left, dynamic right)
        {
            return (left / right);
        }*/
    }
}
