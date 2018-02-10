using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public abstract class CallInstruction : MultiByteInstruction
    {
        protected int _functionNameIndex;

        public abstract string Function { get; }

        public NewListIns Arguments { get; private set; }

        public CallInstruction(OPCode op, bool isMulti, ShockwaveReader input, LingoHandler handler)
            : base(input, handler, op, isMulti)
        {
            _functionNameIndex = _value;
        }

        public override int GetPopCount()
        {
            return 1;
        }

     /*   public override void Translate()
        {
            Arguments = Handler.Expressions.Pop() as NewListIns; //TODO: Check -> if not -> bye bitch
            IsStatement = Arguments == null;
        }*/

        public override string ToString()
            => Function + '(' + Arguments.ToString(false) + ')';
    }
}
