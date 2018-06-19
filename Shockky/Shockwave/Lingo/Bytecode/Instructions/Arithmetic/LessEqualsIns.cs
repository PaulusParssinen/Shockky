using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    class LessEqualsIns : Computation
    {
        public LessEqualsIns() 
            : base(OPCode.LessThanEquals)
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return (left <= right);
        }*/
    }
}
