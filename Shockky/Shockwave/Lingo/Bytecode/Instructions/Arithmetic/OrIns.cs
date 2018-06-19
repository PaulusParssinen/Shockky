using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class OrIns : Computation
    {
        public OrIns()
            : base(OPCode.Or)
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return left || right; //TODO: Those are statements you fucker but this migth be ok for now
        }*/
    }
}
