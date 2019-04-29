namespace Shockky.Lingo.Bytecode.Instructions
{
    public class JumpIns : Jumper
    {
        public JumpIns(LingoHandler handler)
            : base(OPCode.Jump, handler)
        { }
        public JumpIns(LingoHandler handler, int offset)
            : this(handler)
        {
            Value = offset;
        }

        public override bool? RunCondition(LingoMachine machine) => true;
    }
}