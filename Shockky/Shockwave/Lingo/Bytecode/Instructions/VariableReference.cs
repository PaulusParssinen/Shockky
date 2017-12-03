using Shockky.IO;
using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public abstract class VariableReference : MultiByteInstruction
    {
        protected int _variableIndex
            => _value;

        public abstract string Name { get; }

        protected VariableReference(OPCode op, byte opByte, ShockwaveReader input, LingoHandler handler)
            : base(input, handler, op, opByte > 0x80)
        { }

        public override int GetPushCount()
        {
            return 1;
        }

        public override string ToString()
            => Name;
    }
}
