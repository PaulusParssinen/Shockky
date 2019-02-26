namespace Shockky.Lingo.Bytecode.Instructions
{
    public class WrapListIns : Instruction
    {
        public WrapListIns() 
            : base(OPCode.WrapList)
        { }

        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;
    }
}