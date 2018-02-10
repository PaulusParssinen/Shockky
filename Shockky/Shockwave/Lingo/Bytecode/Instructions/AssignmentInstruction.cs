using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public abstract class AssignmentInstruction : VariableReference
    {
//        public override bool IsStatement => true;

        public Instruction Value { get; private set; }

        public AssignmentInstruction(OPCode op, byte opByte, ShockwaveReader input, LingoHandler handler)
            : base(op, opByte, input, handler)
        { }

        public override int GetPopCount()
        {
            return 1;
        }

   /*     public override void Translate()
        {
            Value = Handler.Expressions.Pop();
        }*/

        public override string ToString()
            => Name + " = " + Value;
        
    }
}
