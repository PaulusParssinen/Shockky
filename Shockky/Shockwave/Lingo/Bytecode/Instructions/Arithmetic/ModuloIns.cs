using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class ModuloIns : Computation
    {
        public ModuloIns(LingoHandler handler)
            : base(OPCode.Modulo, handler, "mod")
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return (left % right);
        }
    }
}
