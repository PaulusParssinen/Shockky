using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class OrIns : Computation
    {
        public OrIns(LingoHandler handler)
            : base(OPCode.Or, handler, "or")
        { }

       /* protected override object Execute(dynamic left, dynamic right)
        {
            return left || right; //TODO: Those are statements you fucker but this migth be ok for now
        }*/
    }
}
