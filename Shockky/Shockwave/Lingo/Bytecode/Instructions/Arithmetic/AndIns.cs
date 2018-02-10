using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class AndIns : Computation
    {
        public AndIns(LingoHandler handler)
            : base(OPCode.And, handler, "and")
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return left && right; //TODO: Those are statements you fucker but this migth be ok for now
        }*/
    }
}
