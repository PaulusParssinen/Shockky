namespace Shockky.Shockwave.Lingo.Bytecode.Instructions
{
    public class JoinStringIns : Instruction
    {
        public JoinStringIns()
            : base(OPCode.JoinString)
        { }

        public override int GetPopCount() => 2;
        public override int GetPushCount() => 1;
    }
}