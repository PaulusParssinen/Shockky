namespace Shockky.Lingo.Instructions
{
    public class NewPropListIns : Instruction
    {
        public NewPropListIns()
            : base(OPCode.NewPropList)
        { }

        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;
    }
}