using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class AddIns : Computation
    {
        public AddIns(LingoHandler handler)
            : base(OPCode.Add, handler, "+")
        { }

        /*protected override object Execute(dynamic left, dynamic right)
        {
            return left + right;
        }*/
    }
}
