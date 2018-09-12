namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class SwapIns : Instruction
    {
        public SwapIns()
            : base(OPCode.Swap)
        { }

        public override int GetPopCount() => 2;
        public override int GetPushCount() => 2;

        public override void Execute(LingoMachine machine)
        {
            object value2 = machine.Values.Pop();
            object value1 = machine.Values.Pop();

            machine.Values.Push(value2);
            machine.Values.Push(value1);
        }
    }
}
