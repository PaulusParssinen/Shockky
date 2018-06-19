using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class GreaterThanIns : Computation
    {
        public GreaterThanIns()
            : base(OPCode.GreaterThan)
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return (left > right);
        }*/
    }
}