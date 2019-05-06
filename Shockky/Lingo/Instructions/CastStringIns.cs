namespace Shockky.Lingo.Instructions
{
    public class CastStringIns : Instruction
    {
        public CastStringIns() 
            : base(OPCode.CastString)
        { }

        public override int GetPopCount() => 1;
        public override int GetPushCount() => 1;
    }
}