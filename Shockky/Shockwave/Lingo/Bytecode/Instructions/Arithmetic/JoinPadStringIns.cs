using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class JoinPadStringIns : Computation
    {
        public JoinPadStringIns(LingoHandler handler) 
            : base(OPCode.JoinPadString, handler, "&&")
        { }

        protected override object Execute(dynamic left, dynamic right)
        {
            return left + " " + right; //absolutely fucking top tier
        }
    }
}
