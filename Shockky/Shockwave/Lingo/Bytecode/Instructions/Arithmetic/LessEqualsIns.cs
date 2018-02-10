using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    class LessEqualsIns : Computation
    {
        public LessEqualsIns(LingoHandler handler) 
            : base(OPCode.LessThanEquals, handler, "<=")
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return (left <= right);
        }*/
    }
}
