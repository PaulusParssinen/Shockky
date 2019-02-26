namespace Shockky.Lingo.Bytecode.Instructions
{
    public class InverseIns : Instruction
    {
        public InverseIns()
            : base(OPCode.Inverse)
        { }

        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;

        public override void Execute(LingoMachine machine)
        {
            int value = (int)machine.Values.Pop(); //TODO: Only integerz?? whatever
            machine.Values.Push(value * -1);
        }
    }
}