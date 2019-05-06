namespace Shockky.Lingo.Instructions
{
    public class DupIns : Instruction
    {
        public DupIns(int value)
            : base(OPCode.Dup)
        {
            Value = value; //TODO: No fucking idea what this is
        }

        public override void Execute(LingoMachine machine)
        {
            object value = machine.Values.Peek();
            machine.Values.Push(value);
        }

        public override int GetPushCount() => 1;
    }
}
