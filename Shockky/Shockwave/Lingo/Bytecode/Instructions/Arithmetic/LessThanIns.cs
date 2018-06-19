using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class LessThanIns : Computation
    {
        public LessThanIns()
            : base(OPCode.LessThan)
        { }

      /*  protected override object Execute(dynamic left, dynamic right)
        {
            return (left < right);
        }*/
    }
}
