namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class NotIns : Instruction
    {
        public NotIns() 
            : base(OPCode.Not)
        { }

        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;

        public override void Execute(LingoMachine machine)
        {
            bool value = (bool)machine.Values.Pop();
            machine.Values.Push(!value);
        }
    }
}