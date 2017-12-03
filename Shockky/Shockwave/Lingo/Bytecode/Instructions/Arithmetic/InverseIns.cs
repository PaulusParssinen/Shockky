using Shockky.Shockwave.Lingo.Bytecode.Instructions.Enum;

namespace Shockky.Shockwave.Lingo.Bytecode.Instructions.Arithmetic
{
    public class InverseIns : Instruction
    {
        public InverseIns(LingoHandler handler)
            : base(OPCode.Inverse, handler)
        { }

        public override int GetPopCount()
        {
            return 1;
        }
        public override int GetPushCount()
        {
            return 1;
        }

        public override void Execute(LingoMachine machine)
        {
            int value = (int)machine.Values.Pop(); //TODO: Only integerz?? whatever
            machine.Values.Push(value * -1);
        }
    }
}
