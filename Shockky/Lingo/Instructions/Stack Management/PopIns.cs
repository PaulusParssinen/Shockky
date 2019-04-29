namespace Shockky.Lingo.Bytecode.Instructions
{
    public class PopIns : Instruction
    {
        public int PopAmount { get; set; }

        public PopIns(int popAmount)
            : base(OPCode.Pop)
        {
            Value = popAmount;
        }

        public override int GetPopCount() => PopAmount;

        public override void Execute(LingoMachine machine)
        {
            for (int i = 0; i < PopAmount; i++)
            {
                machine.Values.Pop();
            }
        }
    }
}
