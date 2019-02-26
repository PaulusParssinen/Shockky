namespace Shockky.Lingo.Bytecode.Instructions
{
    public class DupIns : Instruction
    {
        public DupIns(int value)
            : base(OPCode.Dup)
        {
            System.Console.WriteLine(value.ToString());
        }

        public override void Execute(LingoMachine machine)
        {
            var value = machine.Values.Peek();
            machine.Values.Push(value);
        }

        public override int GetPushCount() => 1;
    }
}
