using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class SubtractIns : Computation
    {
        public SubtractIns(LingoHandler handler) 
            : base(OPCode.Substract, handler, "-")
        { }

     /*   protected override object Execute(dynamic left, dynamic right)
        {
            return left - right;
        }*/
    }
}
